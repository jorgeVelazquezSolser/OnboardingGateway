using Aprecia.Domain.Gateway.Authorization.Dtos;
using Aprecia.Domain.Gateway.Shared.Media;

namespace Aprecia.Domain.Gateway.Authorization.Media;

public abstract class AuthorizationResource: Resource<ParamResponseDto>
{
    protected AuthorizationResource(ParamResponseDto response) : base(response) { }
    protected AuthorizationResource() : base(new ParamResponseDto()) { }

}
