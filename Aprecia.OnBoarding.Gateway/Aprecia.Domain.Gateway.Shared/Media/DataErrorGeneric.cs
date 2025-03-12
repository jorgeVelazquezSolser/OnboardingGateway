using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprecia.Domain.Gateway.Shared.Dtos;

namespace Aprecia.Domain.Gateway.Shared.Media;

public static class DataErrorGeneric
{
    public static TResource Error<TResource, TDto>(
        Exception ex,
        string errorMensaje,
        string errorCodigo,
        int errorNivel,
        string errorDescripcionTecnica
    ) where TResource : Resource<TDto>, new()
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
