using Aprecia.Domain.Gateway.Authorization.Dtos;

namespace Aprecia.Domain.Gateway.Authorization.Media;

public class AuthorizationResourceImpl: AuthorizationResource
{
    public AuthorizationResourceImpl() : base() { }
    public AuthorizationResourceImpl(ParamResponseDto response) : base(response) { }
}
