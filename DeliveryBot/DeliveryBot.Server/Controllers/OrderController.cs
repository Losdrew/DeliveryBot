using AutoMapper;
using DeliveryBot.Server.Controllers.Base;
using DeliveryBot.Server.Features.Order;
using DeliveryBot.Shared.Dto.Error;
using DeliveryBot.Shared.Dto.Order;
using DeliveryBot.Shared.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryBot.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : BaseController
{
    public OrderController(IMapper mapper, IMediator mediator)
        : base(mapper, mediator)
    {
    }

    /// <summary>
    /// Create new order.
    /// </summary>
    /// <param name="request">The request to create new order.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <remarks>
    /// If the operation is successful, it will return a OrderInfoDto.
    /// If there is a bad request, it will return an ErrorDto.
    /// </remarks>
    /// <returns>An IActionResult representing the result of the operation.</returns>
    [HttpPost("create")]
    [Authorize(Roles = Roles.Customer)]
    [ProducesResponseType(typeof(OrderInfoDto), 200)]
    [ProducesResponseType(typeof(ErrorDto), 400)]
    [ProducesResponseType(typeof(string), 401)]
    [ProducesResponseType(typeof(string), 403)]
    public async Task<IActionResult> CreateOrder(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(request, cancellationToken);
        return ConvertFromServiceResponse(result);
    }

    /// <summary>
    /// Get user's orders.
    /// </summary>
    /// <remarks>
    /// If the operation is successful, it will return an ICollection of OrderInfoDto.
    /// If there is a bad request, it will return an ErrorDto.
    /// </remarks>
    /// <returns>An IActionResult representing the result of the operation.</returns>
    [HttpGet("user-orders")]
    [Authorize(Roles = Roles.Customer)]
    [ProducesResponseType(typeof(OrderInfoDto), 200)]
    [ProducesResponseType(typeof(ErrorDto), 400)]
    public async Task<IActionResult> GetOwnOrders()
    {
        var query = new GetOwnOrdersQuery();
        var result = await Mediator.Send(query);
        return ConvertFromServiceResponse(result);
    }

    /// <summary>
    /// Get a list of pending orders.
    /// </summary>
    /// <remarks>
    /// If the operation is successful, it will return an ICollection of OrderInfoDto.
    /// If there is a bad request, it will return an ErrorDto.
    /// </remarks>
    /// <returns>An IActionResult representing the result of the operation.</returns>
    [HttpGet("pending-orders")]
    [Authorize(Roles = Roles.CompanyEmployee)]
    [ProducesResponseType(typeof(OrderInfoDto), 200)]
    [ProducesResponseType(typeof(ErrorDto), 400)]
    public async Task<IActionResult> GetAllPendingOrders()
    {
        var query = new GetPendingOrdersQuery();
        var result = await Mediator.Send(query);
        return ConvertFromServiceResponse(result);
    }

    /// <summary>
    /// Cancel own order.
    /// </summary>
    /// <remarks>
    /// If the operation is successful, it will return an OrderInfoDto.
    /// If there is a bad request, it will return an ErrorDto.
    /// </remarks>
    /// <returns>An IActionResult representing the result of the operation.</returns>
    [HttpPost("cancel")]
    [Authorize(Roles = Roles.Customer)]
    [ProducesResponseType(typeof(OrderInfoDto), 200)]
    [ProducesResponseType(typeof(ErrorDto), 400)]
    public async Task<IActionResult> CancelOwnOrder(CancelOwnOrderCommand request, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(request, cancellationToken);
        return ConvertFromServiceResponse(result);
    }
}