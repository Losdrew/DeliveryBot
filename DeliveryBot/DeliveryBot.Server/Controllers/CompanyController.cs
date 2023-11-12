using AutoMapper;
using DeliveryBot.Server.Controllers.Base;
using DeliveryBot.Server.Features.Company;
using DeliveryBot.Shared.Dto.Company;
using DeliveryBot.Shared.Dto.Error;
using DeliveryBot.Shared.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryBot.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CompanyController : BaseController
{
    private readonly IMediator _mediator;

    public CompanyController(IMediator mediator, IMapper mapper)
        : base(mapper)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Create new company.
    /// </summary>
    /// <param name="request">The request to create new company.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <remarks>
    /// If the operation is successful, it will return an OwnCompanyInfoDto.
    /// If there is a bad request, it will return an ErrorDto.
    /// </remarks>
    /// <returns>An IActionResult representing the result of the operation.</returns>
    [HttpPost("create")]
    [Authorize(Roles = Roles.Manager)]
    [ProducesResponseType(typeof(OwnCompanyInfoDto), 200)]
    [ProducesResponseType(typeof(ErrorDto), 400)]
    [ProducesResponseType(typeof(string), 401)]
    [ProducesResponseType(typeof(string), 403)]
    public async Task<IActionResult> CreateCompany(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return ConvertFromServiceResponse(result);
    }

    /// <summary>
    /// Get user's company.
    /// </summary>
    /// <remarks>
    /// If the operation is successful, it will return an OwnCompanyInfoDto.
    /// If there is a bad request, it will return an ErrorDto.
    /// </remarks>
    /// <returns>An IActionResult representing the result of the operation.</returns>
    [HttpGet("user-company")]
    [Authorize(Roles = Roles.Manager)]
    [ProducesResponseType(typeof(OwnCompanyInfoDto), 200)]
    [ProducesResponseType(typeof(ErrorDto), 400)]
    public async Task<IActionResult> GetOwnCompany()
    {
        var query = new GetOwnCompanyQuery();
        var result = await _mediator.Send(query);
        return ConvertFromServiceResponse(result);
    }

    /// <summary>
    /// Get a list of companies.
    /// </summary>
    /// <remarks>
    /// If the operation is successful, it will return an ICollection of CompanyPreviewDto.
    /// If there is a bad request, it will return an ErrorDto.
    /// </remarks>
    /// <returns>An IActionResult representing the result of the operation.</returns>
    [HttpGet("companies")]
    [ProducesResponseType(typeof(CompanyPreviewDto), 200)]
    [ProducesResponseType(typeof(ErrorDto), 400)]
    public async Task<IActionResult> GetCompanies()
    {
        var query = new GetCompaniesQuery();
        var result = await _mediator.Send(query);
        return ConvertFromServiceResponse(result);
    }
}