using Aprecia.Domain.Gateway.Test.Dtos;

namespace Aprecia.OnBoarding.Gateway.Test.Storage;

public interface ITestStorage
{
    Task<IEnumerable<ResponseTestDto>> GetTest(CancellationToken cancellationToken);
}
