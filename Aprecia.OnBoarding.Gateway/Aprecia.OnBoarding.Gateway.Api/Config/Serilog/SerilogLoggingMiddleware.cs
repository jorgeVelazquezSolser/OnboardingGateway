using Aprecia.OnBoarding.Gateway.Api.Config.Serilog;
using Newtonsoft.Json.Linq;
using Serilog;
using System.Text;

public class SerilogLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public SerilogLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {

        if (!context.Request.Path.StartsWithSegments(ConstSerilogMiddleware.Sales_Executive))
        {
            await _next(context);
            return; 
        }
        var logIdBusqueda = Guid.NewGuid();
        context.Items["LogIdBusqueda"] = logIdBusqueda;

        var requestBody = await ReadRequestBody(context.Request);
        var originalBodyStream = context.Response.Body;

        using (var responseBodyStream = new MemoryStream())
        {
            context.Response.Body = responseBodyStream;
            await _next(context);

            string controllerName = "UnknownController";
            string actionName = "UnknownMethod";

            var endpoint = context.GetEndpoint();
            if (endpoint?.Metadata.GetMetadata<Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor>() is { } descriptor)
            {
                controllerName = descriptor.ControllerName;
                actionName = descriptor.ActionName;
            }

            var responseBody = await ReadResponseBody(context.Response);
            var statusCode = context.Response.StatusCode;
            var statusException = context.Items["ExceptionStatus"];
            string? statusCodeException = statusException?.ToString() ?? null;

            if (statusCode == 500 || !string.IsNullOrEmpty(statusCodeException))
            {
                var exceptionObj = context.Items["Exception"];
                string exceptionMessage = exceptionObj?.ToString() ?? "NO EXCEPTION";
                Exception exception = exceptionObj as Exception;

                Log.Error(exception, "HTTP {Method} {Url} - Status: {StatusCode}\nLogIdBusqueda: {LogIdBusqueda}\nAction: {Action}\nRequest: {Request}\nResponse: {Response}",
                        context.Request.Method,
                        $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}",
                        statusCode,
                        logIdBusqueda,
                        actionName,
                        string.IsNullOrEmpty(requestBody) ? "EMPTY REQUEST" : requestBody,
                        string.IsNullOrEmpty(responseBody) ? "EMPTY RESPONSE" : responseBody); 
            }
            else
            {
                Log.Information("HTTP {Method} {Url} - Status: {StatusCode}\nLogIdBusqueda: {LogIdBusqueda}\nAction: {Action}\nRequest: {Request}\nResponse: {Response}",
               context.Request.Method,
               $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}",
               statusCode,
               logIdBusqueda,
               actionName,
               string.IsNullOrEmpty(requestBody) ? "EMPTY REQUEST" : requestBody,
               string.IsNullOrEmpty(responseBody) ? "EMPTY RESPONSE" : responseBody);

            }


            responseBodyStream.Seek(0, SeekOrigin.Begin);
            await responseBodyStream.CopyToAsync(originalBodyStream);
        }
    }

    private async Task<string> ReadRequestBody(HttpRequest request)
    {
        try
        {
            request.EnableBuffering();
            using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            request.Body.Position = 0;
            return string.IsNullOrWhiteSpace(body) ? "EMPTY REQUEST" : body;
        }
        catch
        {
            return "FAILED TO READ REQUEST";
        }
    }

    private async Task<string> ReadResponseBody(HttpResponse response)
    {
        try
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(response.Body, Encoding.UTF8, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return string.IsNullOrWhiteSpace(body) ? "EMPTY RESPONSE" : body;
        }
        catch
        {
            return "FAILED TO READ RESPONSE";
        }
    }
    private string ExtractException(string responseBody)
    {
        try
        {
            var json = JObject.Parse(responseBody);
            return json["Exception"]?.ToString() ?? "";
        }
        catch
        {
            return "";
        }
    }
}
