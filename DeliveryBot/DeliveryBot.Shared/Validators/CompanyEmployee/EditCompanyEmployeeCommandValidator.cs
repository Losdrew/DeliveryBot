using DeliveryBot.Shared.Dto.CompanyEmployee;
using FluentValidation;

namespace DeliveryBot.Shared.Validators.CompanyEmployee;

public class EditCompanyEmployeeCommandValidator : AbstractValidator<EditCompanyEmployeeCommandDto>
{
    public EditCompanyEmployeeCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();

        RuleFor(c => c)
            .SetValidator(new EditableCompanyEmployeeValidator());
    }
}