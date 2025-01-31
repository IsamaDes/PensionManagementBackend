using FluentValidation;
using Pension.Domain.Entities;
using Pension.Application.Dtos;

public class MemberValidator : AbstractValidator<MemberDto>
{
    public MemberValidator()
    {
        RuleFor(m => m.FirstName).NotEmpty().WithMessage("First name is required.");
        RuleFor(m => m.LastName).NotEmpty().WithMessage("Last name is required.");
        RuleFor(m => m.DateOfBirth).NotEmpty().Must(BeValidAge).WithMessage("Age must be between 18 and 70.");
    }

    private bool BeValidAge(DateTime dateOfBirth)
    {
        var age = DateTime.Today.Year - dateOfBirth.Year;
        return age >= 18 && age <= 70;
    }
}
