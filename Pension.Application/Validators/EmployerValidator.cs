using FluentValidation;
using Pension.Domain.Entities;
using Pension.Domain.Repositories;
using System;

namespace Pensions.Application.Validators
{
    public class EmployerValidator : AbstractValidator<Employer>
    {
        public EmployerValidator()
        {
            RuleFor(e => e.CompanyName)
                .NotEmpty().WithMessage("Company name is required.");

            RuleFor(e => e.RegistrationNumber)
                .NotEmpty().WithMessage("Registration number is required.")
                .Matches("^[A-Za-z0-9]{8,12}$").WithMessage("Registration number must be alphanumeric and between 8 to 12 characters.");

            RuleFor(e => e.IsActive)
                .Must(status => status == true || status == false)
                .WithMessage("Active status must be either true or false.");
        }
    }
}
