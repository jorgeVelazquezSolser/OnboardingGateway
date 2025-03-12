using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using MediatR;
using Aprecia.Bussines.Gateway.Authorization.Querys;
using System.Text;
using Serilog;
using Aprecia.Domain.Gateway.Shared.Media;
using Aprecia.Domain.Gateway.Shared.Helper;
using Aprecia.Domain.Gateway.SalesExecutive.Media;
using Aprecia.Domain.Gateway.SalesExecutive.Dtos;
using Aprecia.Domain.Gateway.Authorization.Media;
using Aprecia.Domain.Gateway.Authorization.Dtos;

namespace Aprecia.OnBoarding.Gateway.Api.Configurations;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class Authorization:Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var loggerTxt = Log.ForContext("SourceContext", "AuthorizationLogs");

        if (!context.HttpContext.Items.ContainsKey("LogIdBusqueda"))
        {
            context.HttpContext.Items["LogIdBusqueda"] = Guid.NewGuid().ToString();
        }
        var logIdBusqueda = context.HttpContext.Items["LogIdBusqueda"].ToString()!;        

        var resource = new Resource<object?>();
        resource.Data = null;
        resource.StatusService = new Domain.Gateway.Shared.Dtos.StatusServiceDto();
        resource.StatusService.Id = logIdBusqueda;
        resource.StatusService.Status = false;
        resource.StatusCode = 401;
        resource.StatusService.Error = new Domain.Gateway.Shared.Dtos.ErrorDto();

        try
        {
            


            var configuration = context.HttpContext.RequestServices.GetService<IConfiguration>();
            if (configuration == null)
            {
                resource.StatusService.Error.ErrorMensaje = "No se pudo obtener la configuración del sistema.";
                resource.StatusService.Error.ErrorCodigo = "AAI01";
                resource.StatusService.Error.ErrorNivel = 0;
                resource.StatusService.Error.ErrorDescripcionTecnica = "Error en  Iconfiguraton del sistema";

                context.Result = new UnauthorizedObjectResult(resource.ToSerializableObject());
                return;
            }

            if (!context.HttpContext.Request.Headers.TryGetValue("CERT", out StringValues certBase64))
            {
                resource.StatusService.Error.ErrorMensaje = "El header CERT con el certificado PFX es requerido.";
                resource.StatusService.Error.ErrorCodigo = "AAI02";
                resource.StatusService.Error.ErrorNivel = 0;
                resource.StatusService.Error.ErrorDescripcionTecnica = "Error no encontro el certificado pfx.";

                context.Result = new UnauthorizedObjectResult(resource.ToSerializableObject());
                return;
            }
            
            byte[] pfxBytes = Convert.FromBase64String(certBase64);
            string? certPassword = configuration["PasswordPfx"];

            var certFromHeaders = new X509Certificate2(pfxBytes, certPassword,
                X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);
            
            loggerTxt.Information("📄 Resultado certFromHeaders: {@Resultado}", certFromHeaders);

            if (!certFromHeaders.HasPrivateKey)
            {
                resource.StatusService.Error.ErrorMensaje = "El certificado PFX enviado no contiene clave privada.";
                resource.StatusService.Error.ErrorCodigo = "AAI03";
                resource.StatusService.Error.ErrorNivel = 0;
                resource.StatusService.Error.ErrorDescripcionTecnica = "Error El certificado pfx no es el correcto porque no tiene la llave privada correcta.";

                context.Result = new UnauthorizedObjectResult(resource.ToSerializableObject());
                return;
            }

            var mediator = context.HttpContext.RequestServices.GetService<IMediator>();
            if (mediator == null)
            {
                resource.StatusService.Error.ErrorMensaje = "No se pudo resolver Mediator.";
                resource.StatusService.Error.ErrorCodigo = "AAI04";
                resource.StatusService.Error.ErrorNivel = 0;
                resource.StatusService.Error.ErrorDescripcionTecnica = "No se inicializo correctamente Mediator.";
                context.Result = new UnauthorizedObjectResult(resource.ToSerializableObject());
                return;
            }

            string thumbprint = certFromHeaders.Thumbprint;

            loggerTxt.Information("🔑 Certificado Thumbprint: {Thumbprint}", thumbprint);
            // aqui serilog info de  thumbprint
            var resultadoMediatR = MediatorAuthentication.SendWithLogId<AuthorizationResourceImpl, ParamResponseDto>(context.HttpContext, new GetPrivateKeyQuery(thumbprint),"AMDL01").Result;
            // serilog de  resultadoMediatR
            if (resultadoMediatR == null) 
            {
                context.HttpContext.Items["Exception"] = resultadoMediatR.Exception;
                context.HttpContext.Items["ExceptionStatus"] = "500";
                resource.StatusService.Error.ErrorMensaje = "No se encontro la llave`.";
                resource.StatusService.Error.ErrorCodigo = "AAI07";
                resource.StatusService.Error.ErrorNivel = 0;
                resource.StatusService.Error.ErrorDescripcionTecnica = "No se encontro la llave del pfx.";
                context.Result = new UnauthorizedObjectResult(resource.ToSerializableObject());
                return;
            }

            loggerTxt.Information("📄 Resultado Mediator: {@Resultado}", resultadoMediatR);

            if (resultadoMediatR.StatusCode == 500) 
            {
                context.HttpContext.Items["Exception"] = resultadoMediatR.Exception;
                context.HttpContext.Items["ExceptionStatus"] = "500";
               context.Result = new UnauthorizedObjectResult(resultadoMediatR.ToSerializableObject());
                return;

            }

            using RSA rsaPfx = certFromHeaders.GetRSAPrivateKey();

            loggerTxt.Information("📄 Resultado rsaPfx: {@Resultado}", rsaPfx);

            if (rsaPfx == null)
            {
                throw new Exception("No se pudo obtener la clave privada del certificado.");
            }
            using RSA rsaBase64 = LoadPrivateKeyFromBase64(resultadoMediatR.Data.Value);

            loggerTxt.Information("📄 Resultado rsaBase64: {@Resultado}", rsaBase64);


            if (!ValidatePrivateKeys(rsaPfx, rsaBase64))
            {
                resource.StatusService.Error.ErrorMensaje = "La clave del certificado PFX no es valida.";
                resource.StatusService.Error.ErrorCodigo = "AAI05";
                resource.StatusService.Error.ErrorNivel = 0;
                resource.StatusService.Error.ErrorDescripcionTecnica = "La clave privada del certificado PFX esta erronea o no es el pfx correcto";
                context.Result = new UnauthorizedObjectResult(resource.ToSerializableObject());
                return;
            }

            
        }
        catch (Exception ex)
        {            
            context.HttpContext.Items["Exception"] = ex;
            context.HttpContext.Items["ExceptionStatus"] = "500";
            resource.StatusService.Error.ErrorMensaje = "Error en la autenticación del certificado.";
            resource.StatusService.Error.ErrorCodigo = "AAI06";
            resource.StatusService.Error.ErrorNivel = 0;
            resource.StatusService.Error.ErrorDescripcionTecnica = "Error en la autenticación del certificado";
            context.Result = new UnauthorizedObjectResult(resource.ToSerializableObject());
            return;
        }
    }
    private RSA LoadPrivateKeyFromBase64(string privateKeyBase64)
    {
        try
        {           
            byte[] utf8Bytes = Convert.FromBase64String(privateKeyBase64);
            string utf8String = Encoding.UTF8.GetString(utf8Bytes);
        
            string cleanedBase64 = CleanPEMHeaders(utf8String);

        
            byte[] privateKeyBytes = Convert.FromBase64String(cleanedBase64);
            
            RSA rsa = RSA.Create();
        
            rsa.ImportRSAPrivateKey(privateKeyBytes, out _);
         
            return rsa;
        }
        catch (Exception ex)
        {            
            throw;
        }
    }
    private static string CleanPEMHeaders(string keyContent)
    {
        return keyContent
            .Replace("-----BEGIN RSA PRIVATE KEY-----", "")
            .Replace("-----END RSA PRIVATE KEY-----", "")
            .Replace("\n", "")
            .Replace("\r", "")
            .Trim();
    }
    private bool ValidatePrivateKeys(RSA rsaPfx, RSA rsaBase64)
    {
        try
        {            
            byte[] testData = new byte[] { 1, 2, 3, 4, 5 };
            byte[] signature = rsaPfx.SignData(testData, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

            return rsaBase64.VerifyData(testData, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
        catch
        {
            return false;
        }
    }
    private void LogSuccess(string message, AuthorizationFilterContext context)
    {
        Log.Information("Authorization Log: {Message} | Method: {Method} | URL: {Url} | StatusCode: {StatusCode}",
            message,
            context.HttpContext.Request.Method,
            context.HttpContext.Request.Path,
            200);
    }

    private void LogWarning(string message, AuthorizationFilterContext context)
    {
        Log.Warning("Authorization Log: {Message} | Method: {Method} | URL: {Url} | StatusCode: {StatusCode}",
            message,
            context.HttpContext.Request.Method,
            context.HttpContext.Request.Path,
            401);
    }

    private void LogError(string message, AuthorizationFilterContext context)
    {
        Log.Error("Authorization Log: {Message} | Method: {Method} | URL: {Url} | StatusCode: {StatusCode}",
            message,
            context.HttpContext.Request.Method,
            context.HttpContext.Request.Path,
            500);
    }
}
