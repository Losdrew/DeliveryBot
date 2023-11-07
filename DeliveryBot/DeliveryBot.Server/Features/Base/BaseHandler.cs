using MediatR;

namespace DeliveryBot.Server.Features.Base;

public abstract class BaseHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    protected readonly ILogger<BaseHandler<TRequest, TResponse>> Logger;

    protected BaseHandler(ILogger<BaseHandler<TRequest, TResponse>> logger)
    {
        Logger = logger;
    }

    public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    protected abstract Task<TResponse> UnsafeHandleAsync(TRequest request, CancellationToken cancellationToken);
}