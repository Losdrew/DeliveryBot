﻿using DeliveryBot.Shared.Dto.Company;
using FluentValidation;

namespace DeliveryBot.Shared.Validators.Company;

public class CompanyValidator : AbstractValidator<CompanyDto>
{ 
    public CompanyValidator()
    {
        RuleFor(c => c.Name)
            .NotNull();

        RuleFor(c => c.Description)
            .NotNull();
    }
}