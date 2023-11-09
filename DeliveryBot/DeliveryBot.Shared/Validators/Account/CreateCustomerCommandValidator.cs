using System.Text.RegularExpressions;
using DeliveryBot.Shared.Dto.Account;
using FluentValidation;

namespace DeliveryBot.Shared.Validators.Account;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommandDto>
{ 
    public CreateCustomerCommandValidator()
    {
        RuleFor(c => c.FirstName)
            .NotEmpty();

        RuleFor(c => c.LastName)
            .NotEmpty();

        RuleFor(c => c.PhoneNumber)
            .Matches(new Regex(@"\+\d{12}$"))
            .When(c => !string.IsNullOrEmpty(c.PhoneNumber));
    }
}