using DeliveryBot.Server.Services;
using DeliveryBot.Shared.ServiceResponseHandling;
using MediatR;

namespace DeliveryBot.Server.Mediator.Pipeline;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : ServiceResponse
{
    private readonly IValidationService _validationService;

    public ValidationBehaviour(IValidationService validationService)
    {
        _validationService = validationService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!IsServiceResponse())
        {
            return await next();
        }

        var validationResponse = await _validationService.ValidateAsync(request);

        if (validationResponse.IsSuccess)
        {
            return await next();
        }

        if (IsGenericServiceResponse())
        {
            return (TResponse)MapGenericServiceResponse(validationResponse);
        }

        return (TResponse)validationResponse;
    }

    private bool IsServiceResponse()
    {
        return IsNonGenericServiceResponse() || IsGenericServiceResponse();
    }

    private bool IsNonGenericServiceResponse()
    {
        return typeof(TResponse) == typeof(ServiceResponse);
    }

    private bool IsGenericServiceResponse()
    {
        return typeof(TResponse).IsGenericType &&
               typeof(TResponse).GetGenericTypeDefinition() == typeof(ServiceResponse<>);
    }

    private object MapGenericServiceResponse(ServiceResponse serviceResponse)
    {
        var responseResultType = typeof(TResponse).GenericTypeArguments.Single();

        var mapMethod = typeof(ServiceResponse).GetMethods()
            .First(method => method is
            {
                Name: nameof(ServiceResponse.MapErrorResult),
                IsGenericMethod: true
            });

        var convertedGenericMethod = mapMethod.MakeGenericMethod(responseResultType);
        return convertedGenericMethod.Invoke(serviceResponse, null)!;
    }
}