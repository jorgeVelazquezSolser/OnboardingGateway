using Aprecia.Domain.Gateway.Shared.Media;
using Aprecia.Domain.Gateway.Test.Dtos;

namespace Aprecia.Domain.Gateway.Test.Media;

public class TestResourceList: ResourceList<ResponseTestDto>
{
    public TestResourceList(IEnumerable<ResponseTestDto> response) : base(response) { }
}
