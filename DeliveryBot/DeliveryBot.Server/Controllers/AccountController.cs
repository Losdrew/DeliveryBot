﻿using AutoMapper;
using DeliveryBot.Server.Controllers.Base;
using DeliveryBot.Server.Features.Account;
using DeliveryBot.Shared.Dto.Account;
using DeliveryBot.Shared.Dto.Error;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryBot.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : BaseController
{
    private readonly IMediator _mediator;

    public AccountController(IMediator mediator, IMapper mapper)
        : base(mapper)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Create a new user account.
    /// </summary>
    /// <param name="request">The request to create a user account.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <remarks>
    /// If the operation is successful, it will return a SignUpResultDto.
    /// If there is a bad request, it will return an ErrorDto.
    /// </remarks>
    /// <returns>An IActionResult representing the result of the operation.</returns>
    [HttpPost("user/create")]
    [ProducesResponseType(typeof(SignUpResultDto), 200)]
    [ProducesResponseType(typeof(ErrorDto), 400)]
    public async Task<IActionResult> SignUpUser(CreateIdentityUserCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return ConvertFromServiceResponse(result);
    }

    /// <summary>
    /// Create a new manager account.
    /// </summary>
    /// <param name="request">The request to create a manager account.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <remarks>
    /// If the operation is successful, it will return a SignUpResultDto.
    /// If there is a bad request, it will return an ErrorDto.
    /// </remarks>
    /// <returns>An IActionResult representing the result of the operation.</returns>
    [HttpPost("manager/create")]
    [ProducesResponseType(typeof(SignUpResultDto), 200)]
    [ProducesResponseType(typeof(ErrorDto), 400)]
    public async Task<IActionResult> SignUpManager(CreateManagerCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return ConvertFromServiceResponse(result);
    }

    /// <summary>
    /// Create a new company employee account.
    /// </summary>
    /// <param name="request">The request to create a company employee account.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <remarks>
    /// If the operation is successful, it will return a SignUpResultDto.
    /// If there is a bad request, it will return an ErrorDto.
    /// </remarks>
    /// <returns>An IActionResult representing the result of the operation.</returns>
    [HttpPost("company-employee/create")]
    [ProducesResponseType(typeof(SignUpResultDto), 200)]
    [ProducesResponseType(typeof(ErrorDto), 400)]
    public async Task<IActionResult> SignUpCompanyEmployee(CreateCompanyEmployeeCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return ConvertFromServiceResponse(result);
    }

    /// <summary>
    /// Create a new customer account.
    /// </summary>
    /// <param name="request">The request to create a customer account.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <remarks>
    /// If the operation is successful, it will return a SignUpResultDto.
    /// If there is a bad request, it will return an ErrorDto.
    /// </remarks>
    /// <returns>An IActionResult representing the result of the operation.</returns>
    [HttpPost("customer/create")]
    [ProducesResponseType(typeof(SignUpResultDto), 200)]
    [ProducesResponseType(typeof(ErrorDto), 400)]
    public async Task<IActionResult> SignUpCustomer(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return ConvertFromServiceResponse(result);
    }
}