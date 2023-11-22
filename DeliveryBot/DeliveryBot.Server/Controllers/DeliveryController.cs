using AutoMapper;
using DeliveryBot.Server.Controllers.Base;
using DeliveryBot.Server.Features.Delivery;
using DeliveryBot.Shared.Dto.Delivery;
using DeliveryBot.Shared.Dto.Error;
using DeliveryBot.Shared.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryBot.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DeliveryController : BaseController
{
    public DeliveryController(IMapper mapper, IMediator mediator) 
        : base(mapper, mediator)
    {
    }
    
    /// <summary>
    /// Create new delivery.
    /// </summary>
    /// <param name="request">The request to create new delivery.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <remarks>
    /// If the operation is successful, it will return a DeliveryInfoDto.
    /// If there is a bad request, it will return an ErrorDto.
    /// </remarks>
    /// <returns>An IActionResult representing the result of the operation.</returns>
    [HttpPost("create")]
    [Authorize(Roles = Roles.CompanyEmployee)]
    [ProducesResponseType(typeof(DeliveryInfoDto), 200)]
    [ProducesResponseType(typeof(ErrorDto), 400)]
    [ProducesResponseType(typeof(string), 401)]
    [ProducesResponseType(typeof(string), 403)]
    public async Task<IActionResult> CreateOrder(CreateDeliveryCommand request, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(request, cancellationToken);
        return ConvertFromServiceResponse(result);
    }

    /// <summary>
    /// Get delivery info for customer's order.
    /// </summary>
    /// <remarks>
    /// If the operation is successful, it will return an DeliveryInfoDto.
    /// If there is a bad request, it will return an ErrorDto.
    /// </remarks>
    /// <returns>An IActionResult representing the result of the operation.</returns>
    [HttpGet("order-delivery")]
    [Authorize(Roles = Roles.Customer)]
    [ProducesResponseType(typeof(DeliveryInfoDto), 200)]
    [ProducesResponseType(typeof(ErrorDto), 400)]
    public async Task<IActionResult> GetDelivery(Guid orderId)
    {
        var query = new GetDeliveryQuery
        {
            OrderId = orderId
        };
        var result = await Mediator.Send(query);
        return ConvertFromServiceResponse(result);
    }
}