using AutoMapper;
using DeliveryBot.Shared.Dto.Error;
using DeliveryBot.Shared.ServiceResponseHandling;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryBot.Server.Controllers.Base;

public abstract class BaseController : ControllerBase
{
    private readonly IMapper _mapper;

    protected BaseController(IMapper mapper)
    {
        _mapper = mapper;
    }

    protected IActionResult ConvertFromServiceResponse(ServiceResponse serviceResponse)
    {
        if (serviceResponse.IsSuccess)
        {
            return Ok();
        }
        var errorDto = _mapper.Map<ErrorDto>(serviceResponse.Error);
        return BadRequest(errorDto);
    }

    protected IActionResult ConvertFromServiceResponse<T>(ServiceResponse<T> serviceResponse)
    {
        if (serviceResponse.IsSuccess)
        {
            return Ok(serviceResponse.Result);
        }
        var errorDto = _mapper.Map<ErrorDto>(serviceResponse.Error);
        return BadRequest(errorDto);
    }
}