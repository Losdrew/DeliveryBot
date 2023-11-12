using DeliveryBot.Shared.Dto.Company;
using DeliveryBot.Shared.Validators.Address;
using DeliveryBot.Shared.Validators.CompanyEmployee;
using FluentValidation;

namespace DeliveryBot.Shared.Validators.Company;

public class EditCompanyCommandValidator : AbstractValidator<EditCompanyCommandDto>
{ 
    public EditCompanyCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .When(c => c.Name != null);

        RuleFor(c => c.Description)
            .NotEmpty()
            .When(c => c.Description != null);

        RuleForEach(c => c.CompanyEmployees)
            .SetValidator(new EditableCompanyEmployeeValidator());

        RuleForEach(c => c.CompanyAddresses)
            .SetValidator(new EditableAddressValidator());
    }
}