using DeliveryBot.Shared.Dto.Account;
using FluentValidation;

namespace DeliveryBot.Shared.Validators.Account
{
    public class CredentialsValidator : AbstractValidator<CredentialsDto>
    {
        public CredentialsValidator()
        {
            RuleFor(c => c.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(c => c.Password)
                .NotEmpty()
                .MinimumLength(8);
        }
    }
}