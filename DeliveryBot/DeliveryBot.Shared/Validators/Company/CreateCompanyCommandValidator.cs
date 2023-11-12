using DeliveryBot.Shared.Dto.Company;
using DeliveryBot.Shared.Validators.Address;
using FluentValidation;

namespace DeliveryBot.Shared.Validators.Company;

public class CreateCompanyCommandValidator : AbstractValidator<CreateCompanyCommandDto>
{ 
    public CreateCompanyCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty();

        RuleFor(c => c.Description)
            .NotEmpty();

        RuleForEach(c => c.CompanyAddresses)
            .SetValidator(new AddressValidator());
    }
}