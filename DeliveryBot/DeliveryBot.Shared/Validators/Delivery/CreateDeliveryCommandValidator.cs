using DeliveryBot.Shared.Dto.Delivery;
using FluentValidation;

namespace DeliveryBot.Shared.Validators.Delivery;

public class CreateDeliveryCommandValidator : AbstractValidator<CreateDeliveryCommandDto>
{
    public CreateDeliveryCommandValidator()
    {
        RuleFor(d => d.OrderId)
            .NotEmpty();

        RuleFor(d => d.RobotId)
            .NotEmpty();

        RuleFor(d => d.ShippedDateTime)
            .NotEmpty()
            .Must(date => date != default(DateTime))
            .WithMessage("Delivery shipped date is required");
    }
}