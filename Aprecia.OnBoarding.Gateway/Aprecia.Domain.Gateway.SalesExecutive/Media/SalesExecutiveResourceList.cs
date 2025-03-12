
using Aprecia.Domain.Gateway.SalesExecutive.Dtos;

namespace Aprecia.Domain.Gateway.SalesExecutive.Media;

public abstract class SalesExecutiveResourceList : ResourceList<GetSalesExecutiveResponseDto>
{
    protected SalesExecutiveResourceList(IEnumerable<GetSalesExecutiveResponseDto> response) : base(response) { }

    protected SalesExecutiveResourceList() : base(new List<GetSalesExecutiveResponseDto>()) { }

}
