﻿using DeliveryBot.Shared.Dto.Product;
using FluentValidation;

namespace DeliveryBot.Shared.Validators.Product;

public class EditProductCommandValidator : AbstractValidator<EditProductCommandDto>
{
    public EditProductCommandValidator()
    {
        RuleFor(p => p.Id)
            .NotEmpty();

        RuleFor(p => p.Name)
            .NotEmpty();

        RuleFor(c => c.Price)
            .GreaterThan(0);

        RuleFor(c => c.VolumeCm3)
            .GreaterThan(0);

        RuleFor(c => c.WeightG)
            .GreaterThan(0);
    }
}