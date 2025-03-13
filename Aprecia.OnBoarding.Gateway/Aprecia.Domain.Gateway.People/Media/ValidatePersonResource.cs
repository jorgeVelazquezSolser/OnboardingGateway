using Aprecia.Domain.Gateway.People.Dto.Response;
using Aprecia.Domain.Gateway.Shared.Media;

namespace Aprecia.Domain.Gateway.People.Media;

public class ValidatePersonResource: Resource<ValidatePersonResponseDto>
{
    protected ValidatePersonResource(ValidatePersonResponseDto response) : base(response) { }

    protected ValidatePersonResource() : base(new ValidatePersonResponseDto()) { }
}
