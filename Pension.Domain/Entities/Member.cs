using FluentValidation;
using System;

namespace Pension.Domain.Entities
{
    public class Member
    {
        public Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required DateTime DateOfBirth { get; set; }
        public required string Email { get; set; }
        public string? PhoneNumber { get; set; }

        // Add a soft delete flag
        public bool IsDeleted { get; set; }
    }

    public class MemberValidator : AbstractValidator<Member>
    {
        public MemberValidator()
        {
            RuleFor(m => m.FirstName)
                .NotEmpty().WithMessage("First Name is required.");

            RuleFor(m => m.LastName)
                .NotEmpty().WithMessage("Last Name is required.");

            RuleFor(m => m.DateOfBirth)
                .NotEmpty().WithMessage("Date of Birth is required.")
                .Must(BeAValidAge).WithMessage("Age must be between 18 and 70 years.");

            RuleFor(m => m.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(m => m.PhoneNumber)
                .Matches(@"^\+?[1-9][0-9]{7,14}$")
                .WithMessage("Invalid phone number format.")
                .When(m => !string.IsNullOrEmpty(m.PhoneNumber));
        }

        private bool BeAValidAge(DateTime dateOfBirth)
        {
            var age = DateTime.Today.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > DateTime.Today.AddYears(-age)) age--;
            return age >= 18 && age <= 70;
        }
    }
}