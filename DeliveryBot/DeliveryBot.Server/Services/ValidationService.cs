using DeliveryBot.Shared.Errors.Base;
using DeliveryBot.Shared.ServiceResponseHandling;
using FluentValidation;
using FluentValidation.Results;

namespace DeliveryBot.Server.Services;

public class ValidationService : IValidationService
{
    private readonly IServiceProvider _serviceProvider;

    public ValidationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<ServiceResponse> ValidateAsync<T>(T item)
    {
        var validator = _serviceProvider.GetService<IValidator<T>>();

        if (validator is null)
        {
            return ServiceResponseBuilder.Success();
        }

        var validationContext = new ValidationContext<T>(item);
        var validationResult = await validator.ValidateAsync(validationContext);

        if (validationResult.IsValid)
        {
            return ServiceResponseBuilder.Success();
        }

        var errors = MapValidationErrors(validationResult.Errors);
        return ServiceResponseBuilder.Failure(errors);
    }

    private List<ValidationError> MapValidationErrors(IEnumerable<ValidationFailure> validationFailures)
    {
        return validationFailures.Select(error => new ValidationError
        {
            FieldCode = error.PropertyName,
            ErrorMessage = error.ErrorMessage
        }).ToList();
    }
}