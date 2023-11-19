using DeliveryBot.Shared.Dto.Order;
using DeliveryBot.Shared.Validators.Address;
using FluentValidation;

namespace DeliveryBot.Shared.Validators.Order;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommandDto>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(o => o.PlacedDateTime)
            .NotEmpty()
            .Must(date => date != default(DateTime))
            .WithMessage("Order placed date is required");

        RuleFor(o => o.OrderAddress)
            .NotEmpty()
            .SetValidator(new AddressValidator());

        RuleForEach(o => o.OrderProducts)
            .NotEmpty()
            .SetValidator(new OrderProductValidator());
    }
}