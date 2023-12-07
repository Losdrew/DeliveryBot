using AutoMapper;
using DeliveryBot.Server.Controllers.Base;
using DeliveryBot.Server.Features.Geolocation;
using DeliveryBot.Shared.Dto.Delivery;
using DeliveryBot.Shared.Dto.Error;
using DeliveryBot.Shared.Dto.Geolocation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryBot.Server.Controllers;

public class GeolocationController : BaseController
{
    public GeolocationController(IMapper mapper, IMediator mediator)
        : base(mapper, mediator)
    {
    }

    /// <summary>
    /// Get routes from one location to another.
    /// </summary>
    /// <remarks>
    /// If the operation is successful, it will return a RoutesDto.
    /// If there is a bad request, it will return an ErrorDto.
    /// </remarks>
    /// <param name="coordinates">The coordinates of two points separated by semicolon</param>
    /// <returns>An IActionResult representing the result of the operation.</returns>
    [HttpGet("route/{coordinates}")]
    [ProducesResponseType(typeof(RoutesDto), 200)]
    [ProducesResponseType(typeof(ErrorDto), 400)]
    public async Task<IActionResult> GetRoute([FromRoute] string coordinates)
    {
        var points = coordinates.Split(';');

        var query = new GetRouteQuery
        {
            FirstPoint = ParseLocation(points[0]),
            SecondPoint = ParseLocation(points[1])
        };

        var result = await Mediator.Send(query);
        return ConvertFromServiceResponse(result);
    }

    private LocationDto ParseLocation(string locationString)
    {
        var location = new LocationDto();
        var coordinates = locationString.Split(',');

        location.X = double.Parse(coordinates[0]);
        location.Y = double.Parse(coordinates[1]);

        return location;
    }
}