
using System.Reflection;
using Aprecia.Domain.Gateway.Shared.Media;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Aprecia.Domain.Gateway.Shared.Helper;

public static  class MediatorAuthentication
{
    private static IMediator GetMediator(HttpContext context)
    {
        return context.RequestServices.GetService<IMediator>()!;
    }

    public static string GetLogIdBusqueda(HttpContext context)
    {
        if (!context.Items.ContainsKey("LogIdBusqueda"))
        {
            context.Items["LogIdBusqueda"] = Guid.NewGuid().ToString();
        }
        return context.Items["LogIdBusqueda"].ToString()!;
    }
    public static async Task<T> SendWithLogId<T, Z>(HttpContext context, IRequest<T> request, string codigoController)
           where T : Resource<Z>, new()
    {
        try
        {
            var mediator = GetMediator(context); // Obtener Mediator
            var result = await mediator.Send(request);
            AssignLogIdIfExists(context, result);

            if (result.StatusCode == 500 && result.Exception != null)
            {
                context.Items["Exception"] = result.Exception;
            }
            return result;
        }
        catch (Exception ex)
        {
            context.Items["Exception"] = ex;

            var resultError = Activator.CreateInstance<T>();
            resultError = DataErrorGeneric.Error<T, Z>(
                ex,
                "Error inesperado, contacte al administrador",
                codigoController,
                3,
                "Error inesperado, revise los logs"
            );

            AssignLogIdIfExists(context, resultError);
            return resultError;
        }
    }
    public static async Task<T> SendWithLogIdList<T, Z>(HttpContext context, IRequest<T> request, string codigoController)
            where T : ResourceList<Z>, new()
    {
        try
        {
            var mediator = GetMediator(context); // Obtener Mediator
            var result = await mediator.Send(request);
            AssignLogIdIfExists(context, result);

            if (result.StatusCode == 500 && result.Exception != null)
            {
                context.Items["Exception"] = result.Exception;
            }
            return result;
        }
        catch (Exception ex)
        {
            context.Items["Exception"] = ex;

            var resultError = Activator.CreateInstance<T>();
            resultError = DataErrorGenericList.Error<T, Z>(
                ex,
                "Error inesperado, contacte al administrador",
                codigoController,
                3,
                "Error inesperado, revise los logs"
            );

            AssignLogIdIfExists(context, resultError);
            return resultError;
        }
    }

    private static void AssignLogIdIfExists<T>(HttpContext context, T result)
    {
        if (result == null) return;

        var statusServiceProperty = typeof(T).GetProperty("StatusService", BindingFlags.Public | BindingFlags.Instance);
        if (statusServiceProperty != null)
        {
            var statusServiceInstance = statusServiceProperty.GetValue(result);
            if (statusServiceInstance != null)
            {
                var idProperty = statusServiceInstance.GetType().GetProperty("Id", BindingFlags.Public | BindingFlags.Instance);
                if (idProperty != null && idProperty.PropertyType == typeof(string) && idProperty.CanWrite)
                {
                    idProperty.SetValue(statusServiceInstance, GetLogIdBusqueda(context));
                    return;
                }
            }
        }
    }
}
