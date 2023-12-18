using DeliveryBot.Shared.Dto.CompanyEmployee;
using FluentValidation;

namespace DeliveryBot.Shared.Validators.CompanyEmployee;

public class CompanyEmployeeValidator : AbstractValidator<CompanyEmployeeDto>
{ 
    public CompanyEmployeeValidator()
    {
        RuleFor(e => e.Email)
            .NotEmpty()
            .EmailAddress()
            .When(c => c.Email != null);

        RuleFor(e => e.FirstName)
            .NotEmpty()
            .When(c => c.FirstName != null);

        RuleFor(e => e.LastName)
            .NotEmpty()
            .When(c => c.LastName != null);
    }
}