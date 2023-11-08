using DeliveryBot.Shared.Dto.Account;
using FluentValidation;

namespace DeliveryBot.Shared.Validators.Account;

public class CompanyEmployeeValidator : AbstractValidator<CompanyEmployeeDto>
{ 
    public CompanyEmployeeValidator()
    {
        RuleFor(e => e.FirstName)
            .NotNull();

        RuleFor(e => e.LastName)
            .NotNull();
    }
}