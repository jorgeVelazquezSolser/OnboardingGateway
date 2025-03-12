using Aprecia.Domain.Gateway.Shared.Dtos;

namespace Aprecia.Domain.Gateway.Shared.Media;

public static class DataErrorGenericList
{
    public static TResource Error<TResource, TDto>(
        Exception ex,
        string errorMensaje,
        string errorCodigo,
        int errorNivel,
        string errorDescripcionTecnica
    ) where TResource : ResourceList<TDto>, new()
    {
        var error = new TResource();
        error.StatusCode = 500;
        error.Exception = ex; 
        error.StatusService = new StatusServiceDto()
        {
            Status = false,
            Error = new ErrorDto()
            {
                ErrorMensaje = errorMensaje,
                ErrorCodigo = errorCodigo,
                ErrorNivel = errorNivel,
                ErrorDescripcionTecnica = errorDescripcionTecnica
            }
        };

        return error;
    }
}


