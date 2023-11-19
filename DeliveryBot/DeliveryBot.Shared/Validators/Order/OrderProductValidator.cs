using DeliveryBot.Shared.Dto.Order;
using FluentValidation;

namespace DeliveryBot.Shared.Validators.Order;

public class OrderProductValidator : AbstractValidator<OrderProductDto>
{
    public OrderProductValidator()
    {
        RuleFor(op => op.ProductId)
            .NotEmpty();

        RuleFor(op => op.Quantity)
            .GreaterThan(0);
    }
}