using Aprecia.Domain.Gateway.Authorization.Dtos;

namespace Aprecia.Bussines.Gateway.Authorization.Storage;

public interface IAuthorizationStorage
{
    Task<ParamResponseDto> GetParamsByKey(string key, CancellationToken cancellationToken);
}
