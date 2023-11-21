using AutoMapper;
using DeliveryBot.Server.Controllers.Base;
using DeliveryBot.Server.Features.Product;
using DeliveryBot.Server.Features.Robot;
using DeliveryBot.Shared.Dto.Error;
using DeliveryBot.Shared.Dto.Product;
using DeliveryBot.Shared.Dto.Robot;
using DeliveryBot.Shared.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryBot.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RobotController : BaseController
{
    public RobotController(IMapper mapper, IMediator mediator) 
        : base(mapper, mediator)
    {
    }

    /// <summary>
    /// Create new robot.
    /// </summary>
    /// <param name="request">The request to create new robot.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <remarks>
    /// If the operation is successful, it will return a RobotDto.
    /// If there is a bad request, it will return an ErrorDto.
    /// </remarks>
    /// <returns>An IActionResult representing the result of the operation.</returns>
    [HttpPost("create")]
    [Authorize(Roles = Roles.Administrator)]
    [ProducesResponseType(typeof(RobotDto), 200)]
    [ProducesResponseType(typeof(ErrorDto), 400)]
    [ProducesResponseType(typeof(string), 401)]
    [ProducesResponseType(typeof(string), 403)]
    public async Task<IActionResult> CreateRobot(CreateRobotCommand request, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(request, cancellationToken);
        return ConvertFromServiceResponse(result);
    }
}