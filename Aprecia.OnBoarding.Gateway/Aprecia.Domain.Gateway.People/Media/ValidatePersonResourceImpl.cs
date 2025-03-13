using Aprecia.Domain.Gateway.People.Dto.Response;

namespace Aprecia.Domain.Gateway.People.Media;

public class ValidatePersonResourceImpl: ValidatePersonResource
{
    public ValidatePersonResourceImpl() : base() { }

    public ValidatePersonResourceImpl(ValidatePersonResponseDto response) : base(response) { }
    
}
