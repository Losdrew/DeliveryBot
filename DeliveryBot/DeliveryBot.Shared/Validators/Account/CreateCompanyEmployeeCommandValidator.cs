using DeliveryBot.Shared.Dto.Account;
using FluentValidation;

namespace DeliveryBot.Shared.Validators.Account;

public class CreateCompanyEmployeeCommandValidator : AbstractValidator<CreateCompanyEmployeeCommandDto>
{ 
    public CreateCompanyEmployeeCommandValidator()
    {
        RuleFor(c => c.FirstName)
            .NotNull();

        RuleFor(c => c.LastName)
            .NotNull();
    }
}