using AutoMapper;
using DeliveryBot.Server.Controllers.Base;
using MediatR;
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
}