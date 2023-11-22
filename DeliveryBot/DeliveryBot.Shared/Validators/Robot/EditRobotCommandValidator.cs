using DeliveryBot.Shared.Dto.Robot;
using FluentValidation;

namespace DeliveryBot.Shared.Validators.Robot;

public class EditRobotCommandValidator : AbstractValidator<EditRobotCommandDto>
{
    public EditRobotCommandValidator()
    {
        RuleFor(r => r.Id)
            .NotEmpty();

        RuleFor(r => r.Name)
            .NotEmpty();

        RuleFor(r => r.VolumeCapacityCm3)
            .GreaterThan(0);

        RuleFor(r => r.WeightCapacityG)
            .GreaterThan(0);
    }
}