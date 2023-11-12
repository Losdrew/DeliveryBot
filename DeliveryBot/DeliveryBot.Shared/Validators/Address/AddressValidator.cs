using DeliveryBot.Shared.Dto.Address;
using FluentValidation;

namespace DeliveryBot.Shared.Validators.Address;

public class AddressValidator : AbstractValidator<AddressDto>
{
    public AddressValidator()
    {
        RuleFor(a => a.AddressLine1).NotEmpty();
        RuleFor(a => a.AddressLine2).NotEmpty();
        RuleFor(a => a.TownCity).NotEmpty();
        RuleFor(a => a.Region).NotEmpty();
        RuleFor(a => a.Country).NotEmpty();
        RuleFor(a => a.Postcode).NotEmpty();
    }
}