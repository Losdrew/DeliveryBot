using AutoMapper;
using DeliveryBot.Db.DbContexts;
using MediatR;

namespace DeliveryBot.Server.Features.Base;

public abstract class ExtendedBaseHandler<TRequest, TResponse> : BaseHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    protected readonly ApplicationDbContext Context;
    protected readonly IHttpContextAccessor ContextAccessor;
    protected readonly IMapper Mapper;

    protected ExtendedBaseHandler(ApplicationDbContext context, IHttpContextAccessor contextAccessor, IMapper mapper,
        ILogger<ExtendedBaseHandler<TRequest, TResponse>> logger)
        : base(logger)
    {
        Context = context;
        ContextAccessor = contextAccessor;
        Mapper = mapper;
    }
}