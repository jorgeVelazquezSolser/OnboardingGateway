using Aprecia.Domain.Gateway.SalesExecutive.Dtos;

namespace Aprecia.Domain.Gateway.SalesExecutive.Media;

public class SalesExecutiveResourceListImpl : SalesExecutiveResourceList
{
    public SalesExecutiveResourceListImpl() : base() { }
    public SalesExecutiveResourceListImpl(IEnumerable<GetSalesExecutiveResponseDto> response) : base(response) { }
}
