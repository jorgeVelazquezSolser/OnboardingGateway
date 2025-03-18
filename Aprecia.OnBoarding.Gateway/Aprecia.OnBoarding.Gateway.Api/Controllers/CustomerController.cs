using Aprecia.Bussines.Gateway.People.Query;
using Aprecia.Domain.Gateway.People.Dto.Request;
using Aprecia.Domain.Gateway.People.Dto.Response;
using Aprecia.Domain.Gateway.People.Media;
using Aprecia.Domain.Gateway.Shared.Dtos.Request;
using Aprecia.OnBoarding.Gateway.Api.Configurations;
using Microsoft.AspNetCore.Mvc;

namespace Aprecia.OnBoarding.Gateway.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorization]
public class CustomerController : MediatoRController
{
    [HttpPost("Validate")]
    public async Task<IActionResult> ValidatePerson(ResourceRequestBaseDto<DataValidatePersonRquestDto> request)
    {
        var result = await SendWithLogId<ValidatePersonResourceImpl, ValidatePersonResponseDto>(new ValidatePersonQuery(request.Data), "CP01");
        return StatusCode(result.StatusCode, result.ToSerializableObject());
    }

}
