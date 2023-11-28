using AutoMapper;
using DeliveryBot.Server.Controllers.Base;
using DeliveryBot.Server.Features.Product;
using DeliveryBot.Shared.Dto.Error;
using DeliveryBot.Shared.Dto.Product;
using DeliveryBot.Shared.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryBot.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : BaseController
{
    public ProductController(IMapper mapper, IMediator mediator)
        : base(mapper, mediator)
    {
    }

    /// <summary>
    /// Create new product.
    /// </summary>
    /// <param name="request">The request to create new product.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <remarks>
    /// If the operation is successful, it will return a CompanyProductInfoDto.
    /// If there is a bad request, it will return an ErrorDto.
    /// </remarks>
    /// <returns>An IActionResult representing the result of the operation.</returns>
    [HttpPost("create")]
    [Authorize(Roles = Roles.Manager)]
    [ProducesResponseType(typeof(CompanyProductInfoDto), 200)]
    [ProducesResponseType(typeof(ErrorDto), 400)]
    [ProducesResponseType(typeof(string), 401)]
    [ProducesResponseType(typeof(string), 403)]
    public async Task<IActionResult> CreateProduct(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(request, cancellationToken);
        return ConvertFromServiceResponse(result);
    }

    /// <summary>
    /// Get a company's product list.
    /// </summary>
    /// <param name="companyId"></param>
    /// <remarks>
    /// If the operation is successful, it will return an ICollection of CompanyProductInfoDto.
    /// If there is a bad request, it will return an ErrorDto.
    /// </remarks>
    /// <returns>An IActionResult representing the result of the operation.</returns>
    [HttpGet("company-products")]
    [ProducesResponseType(typeof(CompanyProductInfoDto), 200)]
    [ProducesResponseType(typeof(ErrorDto), 400)]
    public async Task<IActionResult> GetCompanyProducts(Guid companyId)
    {
        var query = new GetCompanyProductsQuery
        {
            CompanyId = companyId
        };
        var result = await Mediator.Send(query);
        return ConvertFromServiceResponse(result);
    }

    /// <summary>
    /// Edit existing product.
    /// </summary>
    /// <param name="request">The request to edit company's product.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <remarks>
    /// If the operation is successful, it will return an CompanyProductInfoDto.
    /// If there is a bad request, it will return an ErrorDto.
    /// </remarks>
    /// <returns>An IActionResult representing the result of the operation.</returns>
    [HttpPost("edit")]
    [Authorize(Roles = Roles.Manager)]
    [ProducesResponseType(typeof(CompanyProductInfoDto), 200)]
    [ProducesResponseType(typeof(ErrorDto), 400)]
    [ProducesResponseType(typeof(string), 401)]
    [ProducesResponseType(typeof(string), 403)]
    public async Task<IActionResult> EditProduct(EditProductCommand request, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(request, cancellationToken);
        return ConvertFromServiceResponse(result);
    }

    /// <summary>
    /// Delete existing product.
    /// </summary>
    /// <param name="productId">The request to delete company's product.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <remarks>
    /// If there is a bad request, it will return an ErrorDto.
    /// </remarks>
    /// <returns>An IActionResult representing the result of the operation.</returns>
    [HttpDelete("delete")]
    [Authorize(Roles = Roles.Manager)]
    [ProducesResponseType(typeof(ErrorDto), 400)]
    public async Task<IActionResult> DeleteProduct(Guid productId, CancellationToken cancellationToken)
    {
        var command = new DeleteProductCommand
        {
            ProductId = productId
        };
        var result = await Mediator.Send(command, cancellationToken);
        return ConvertFromServiceResponse(result);
    }
}