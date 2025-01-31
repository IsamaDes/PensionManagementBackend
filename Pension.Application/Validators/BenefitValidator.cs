using FluentValidation;
using Pension.Domain.Entities;
using Pension.Domain.Repositories;
using System;

namespace Pension.Application.Validators
{
    public class BenefitValidator : AbstractValidator<Benefit>
    {
        public BenefitValidator()
        {
            RuleFor(b => b.BenefitType)
                .NotEmpty().WithMessage("Benefit Type is required");

            RuleFor(b => b.CalculationDate)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Calculation date cannot be in the future");

            RuleFor(b => b.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than zero");
        }
    }
}
