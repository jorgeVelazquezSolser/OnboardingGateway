using Aprecia.Bussines.Gateway.People.Base;
using Aprecia.Bussines.Gateway.People.ModelServices;
using Aprecia.Bussines.Gateway.People.Storage;
using Aprecia.Domain.Gateway.People.Dto.Response;
using Aprecia.Domain.Gateway.People.Media;
using Aprecia.Domain.Gateway.Shared.Dtos;
using Aprecia.Domain.Gateway.Shared.Media;
using MediatR;

namespace Aprecia.Bussines.Gateway.People.Query;

public class ValidatePersonQueryHandler : RequestHandlerBase, IRequestHandler<ValidatePersonQuery, ValidatePersonResourceImpl>
{
    public ValidatePersonQueryHandler(IUnitOfWorkFactory unitOfWorkFactory) : base(unitOfWorkFactory) { }
    public async Task<ValidatePersonResourceImpl> Handle(ValidatePersonQuery request, CancellationToken cancellationToken)
    {
        ValidatePersonResponseDto validate = new ValidatePersonResponseDto()
        {
            Nombre = $"{request.validate.PrimerNombre} " +
            $"{(string.IsNullOrEmpty(request.validate.SegundoNombre) ? request.validate.PrimerApellido : request.validate.SegundoNombre)} " +
            $"{(!string.IsNullOrEmpty(request.validate.SegundoNombre) ? request.validate.PrimerApellido : request.validate.SegundoApellido)} " +
            $"{(!string.IsNullOrEmpty(request.validate.SegundoNombre) ? request.validate.SegundoApellido : string.Empty)}",
            Curp = request.validate.Curp,
            TieneCreditos = false,
            PorcentajeCoincidencia = 0
        };

        try
        {
            IEnumerable<PersonGetList> result = await UnitOfWorks.PeopleStorage.GetPersonList(request.validate.Curp);

            if (result.Count() < 1)
            {
                return new ValidatePersonResourceImpl(validate);
            }

            validate.TieneCreditos = result.Any(x => x.cuenta_creditos_activos > 0);

            double maxCoincidencia = result.Max(x => CalcularPorcentajeCoincidencia(validate.Nombre, x.nombre));
            validate.PorcentajeCoincidencia = maxCoincidencia;

            var success = new ValidatePersonResourceImpl(validate);
            success.StatusService = new StatusServiceDto();
            success.StatusService.Status = true;
            return success;
        }
        catch (Exception ex)
        {
            var error = DataErrorGeneric.Error<ValidatePersonResourceImpl, ValidatePersonResponseDto>(
            ex,
                "Error vuelva a intentar de favor",
                "BPV01",
                1,
                "Error al intentar accesar a Storage ValidatePerson"
            );
            error.Data = validate;
            return error;
        }
    }

    private static double CalcularPorcentajeCoincidencia(string nombre1, string nombre2)
    {
        int distancia = DistanciaLevenshtein(nombre1, nombre2);
        int longitudMaxima = Math.Max(nombre1.Length, nombre2.Length);

        if (longitudMaxima == 0) return 100.0;

        return (1.0 - (double)distancia / longitudMaxima) * 100.0;
    }

    private static int DistanciaLevenshtein(string a, string b)
    {
        int n = a.Length;
        int m = b.Length;
        int[,] dp = new int[n + 1, m + 1];

        for (int i = 0; i <= n; i++)
        {
            for (int j = 0; j <= m; j++)
            {
                if (i == 0)
                    dp[i, j] = j;
                else if (j == 0)
                    dp[i, j] = i;
                else
                    dp[i, j] = Math.Min(Math.Min(dp[i - 1, j] + 1, dp[i, j - 1] + 1),
                                        dp[i - 1, j - 1] + (a[i - 1] == b[j - 1] ? 0 : 1));
            }
        }
        return dp[n, m];
    }


}
