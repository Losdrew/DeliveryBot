using AutoMapper;
using DeliveryBot.Server.Controllers.Base;
using DeliveryBot.Server.Features.CompanyEmployee;
using DeliveryBot.Shared.Dto.CompanyEmployee;
using DeliveryBot.Shared.Dto.Error;
using DeliveryBot.Shared.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryBot.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CompanyEmployeeController : BaseController
{
    public CompanyEmployeeController(IMapper mapper, IMediator mediator)
        : base(mapper, mediator)
    {
    }

    /// <summary>
    /// Edit existing company employee.
    /// </summary>
    /// <param name="request">The request to edit company employee.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <remarks>
    /// If the operation is successful, it will return a CompanyEmployeeInfoDto.
    /// If there is a bad request, it will return an ErrorDto.
    /// </remarks>
    /// <returns>An IActionResult representing the result of the operation.</returns>
    [HttpPost("edit")]
    [Authorize(Roles = Roles.Manager)]
    [ProducesResponseType(typeof(CompanyEmployeeInfoDto), 200)]
    [ProducesResponseType(typeof(ErrorDto), 400)]
    [ProducesResponseType(typeof(string), 401)]
    [ProducesResponseType(typeof(string), 403)]
    public async Task<IActionResult> EditCompanyEmployee(EditCompanyEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(request, cancellationToken);
        return ConvertFromServiceResponse(result);
    }

    /// <summary>
    /// Delete existing company employee.
    /// </summary>
    /// <param name="companyEmployeeId">The request to delete company employee.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <remarks>
    /// If there is a bad request, it will return an ErrorDto.
    /// </remarks>
    /// <returns>An IActionResult representing the result of the operation.</returns>
    [HttpDelete]
    [Authorize(Roles = Roles.Manager)]
    [ProducesResponseType(typeof(ErrorDto), 400)]
    public async Task<IActionResult> DeleteCompanyEmployee(Guid companyEmployeeId, CancellationToken cancellationToken)
    {
        var command = new DeleteCompanyEmployeeCommand
        {
            CompanyEmployeeId = companyEmployeeId
        };
        var result = await Mediator.Send(command, cancellationToken);
        return ConvertFromServiceResponse(result);
    }
}