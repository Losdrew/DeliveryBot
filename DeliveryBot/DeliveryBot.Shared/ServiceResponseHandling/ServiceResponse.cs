using DeliveryBot.Shared.Errors.Base;

namespace DeliveryBot.Shared.ServiceResponseHandling;

public class ServiceResponse
{
    public bool IsSuccess { get; set; }
    public Error Error { get; set; } = new Error();

    public ServiceResponse MapErrorResult()
    {
        return new ServiceResponse()
        {
            IsSuccess = false,
            Error = new Error(Error.ValidationErrors, Error.ServiceErrors)
        };
    }

    public ServiceResponse<TDestinationResult> MapErrorResult<TDestinationResult>()
    {
        return new ServiceResponse<TDestinationResult>()
        {
            IsSuccess = false,
            Error = new Error(Error.ValidationErrors, Error.ServiceErrors)
        };
    }
}

public class ServiceResponse<TResult> : ServiceResponse
{
    public TResult Result { get; set; }
}