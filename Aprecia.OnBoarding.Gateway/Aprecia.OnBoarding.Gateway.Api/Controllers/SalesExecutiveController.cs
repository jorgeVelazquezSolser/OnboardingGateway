using Aprecia.Domain.Gateway.SalesExecutive.Dtos;
using Aprecia.Domain.Gateway.SalesExecutive.Media;
using Aprecia.Domain.Gateway.Shared.Dtos.Request;
using Aprecia.OnBoarding.Gateway.Api.Configurations;
using Aprecia.OnBoarding.Gateway.SalesExecutive.Query;
using Microsoft.AspNetCore.Mvc;

namespace Aprecia.OnBoarding.Gateway.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorization]
public class SalesExecutiveController : MediatoRController
{
    [HttpPost("GetList")]
    public async Task<IActionResult> Get(ResourceRequestBaseDto<object?> request)
    {       
        var result = await SendWithLogId<SalesExecutiveResourceListImpl,GetSalesExecutiveResponseDto>(new GetSalesExecutiveQuery(),"CSE01");
        return StatusCode(result.StatusCode, result.ToSerializableObject());
    }
}
