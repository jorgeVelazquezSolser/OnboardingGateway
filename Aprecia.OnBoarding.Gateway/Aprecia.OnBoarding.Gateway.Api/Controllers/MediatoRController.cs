using System.Reflection;
using Aprecia.Domain.Gateway.Shared.Media;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Aprecia.OnBoarding.Gateway.Api.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public abstract class MediatoRController : ControllerBase
    {
        private IMediator _mediator = default!;

        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>()!;

        protected string LogIdBusqueda
        {
            get
            {
                if (!HttpContext.Items.ContainsKey("LogIdBusqueda"))
                {
                    HttpContext.Items["LogIdBusqueda"] = Guid.NewGuid().ToString(); 
                }
                return HttpContext.Items["LogIdBusqueda"].ToString()!;
            }
        }

        protected async Task<T> SendWithLogIdList<T,Z>(IRequest<T> request,string codigoController) where T : ResourceList<Z>, new()
        {
            try
            {
                var result = await Mediator.Send(request);
                AssignLogIdIfExists(result);
                if (result.StatusCode == 500 && result.Exception != null)
                {
                    HttpContext.Items["Exception"] = result.Exception;
                }
                return result;
            }
            catch (Exception ex)
            {
                HttpContext.Items["Exception"] = ex;
                var resultError = Activator.CreateInstance<T>();
                resultError = DataErrorGenericList.Error<T, Z>(
                    ex,
                    "Error inesperado, contacte al administrador",
                    codigoController,
                    3,
                    "Error inesperado, revise los logs"
                );

                AssignLogIdIfExists(resultError);
                return resultError;
            }
        }

        protected async Task<T> SendWithLogId<T, Z>(IRequest<T> request, string codigoController) where T : Resource<Z>, new()
        {
            try
            {
                var result = await Mediator.Send(request);
                AssignLogIdIfExists(result);
                if (result.StatusCode == 500 && result.Exception != null)
                {
                    HttpContext.Items["Exception"] = result.Exception;
                }
                return result;
            }
            catch (Exception ex)
            {
                HttpContext.Items["Exception"] = ex;
                var resultError = Activator.CreateInstance<T>();
                resultError = DataErrorGeneric.Error<T, Z>(
                    ex,
                    "Error inesperado, contacte al administrador",
                    codigoController,
                    3,
                    "Error inesperado, revise los logs"
                );

                AssignLogIdIfExists(resultError);
                return resultError;
            }
        }


        private void AssignLogIdIfExists<T>(T result)
        {
            if (result == null) return;
        
            var statusServiceProperty = typeof(T).GetProperty("StatusService", BindingFlags.Public | BindingFlags.Instance);

            if (statusServiceProperty != null)
            {
                var statusServiceInstance = statusServiceProperty.GetValue(result);
                if (statusServiceInstance != null)
                {
                    // 🔹 Buscar la propiedad `Id` dentro de `StatusService`
                    var idProperty = statusServiceInstance.GetType().GetProperty("Id", BindingFlags.Public | BindingFlags.Instance);
                    if (idProperty != null && idProperty.PropertyType == typeof(string) && idProperty.CanWrite)
                    {
                        idProperty.SetValue(statusServiceInstance, LogIdBusqueda);
                        return;
                    }
                }
            }
        }
    }
}

