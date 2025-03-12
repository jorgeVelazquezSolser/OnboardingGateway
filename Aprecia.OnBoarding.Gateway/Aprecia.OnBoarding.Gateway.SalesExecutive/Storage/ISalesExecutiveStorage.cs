using Aprecia.Domain.Gateway.SalesExecutive.Dtos;

namespace Aprecia.OnBoarding.Gateway.SalesExecutive.Storage;

public interface ISalesExecutiveStorage
{
    Task<IEnumerable<GetSalesExecutiveResponseDto>> GetSalesExecutives(CancellationToken cancellationToken);
}
