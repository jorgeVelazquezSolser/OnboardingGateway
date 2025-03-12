using Aprecia.Bussines.Gateway.Authorization.Base;
using Aprecia.Bussines.Gateway.Authorization.Storage;
using Aprecia.Domain.Gateway.Authorization.Dtos;
using Aprecia.Domain.Gateway.Authorization.Media;
using Aprecia.Domain.Gateway.Shared.Dtos;
using Aprecia.Domain.Gateway.Shared.Media;
using MediatR;

namespace Aprecia.Bussines.Gateway.Authorization.Querys;

public class GetPrivateKeyQueryHandler:RequestHandlerBase, IRequestHandler<GetPrivateKeyQuery, AuthorizationResourceImpl>
{
    public GetPrivateKeyQueryHandler(IUnitOfWorkFactory unitOfWorkFactory) : base(unitOfWorkFactory) { }

    public async Task<AuthorizationResourceImpl> Handle(GetPrivateKeyQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await UnitOfWorks.AuthorizationStorage.GetParamsByKey(request.Thumbprint, cancellationToken);
            var success = new AuthorizationResourceImpl(result);
            success.StatusService = new StatusServiceDto();
            success.StatusService.Status = true;
            return success;
        }
        catch (Exception ex)
        {
            return DataErrorGeneric.Error<AuthorizationResourceImpl, ParamResponseDto>(
            ex,
                "Error vuelva a intentar de favor",
                "AHTDAGPVKQ01",
                1,
                "Error al intentar accesar a Storage GetParamsByKey"
            );

        }
    }
}
