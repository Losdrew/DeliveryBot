using DeliveryBot.Shared.Dto.Robot;
using FluentValidation;

namespace DeliveryBot.Shared.Validators.Robot;

public class CreateRobotCommandValidator : AbstractValidator<CreateRobotCommandDto>
{
    public CreateRobotCommandValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty();

        RuleFor(r => r.VolumeCapacityCm3)
            .GreaterThan(0);

        RuleFor(r => r.WeightCapacityG)
            .GreaterThan(0);
    }
}