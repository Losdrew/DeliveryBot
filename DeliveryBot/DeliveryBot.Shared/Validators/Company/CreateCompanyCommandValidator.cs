using DeliveryBot.Shared.Dto.Company;
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
    }
}