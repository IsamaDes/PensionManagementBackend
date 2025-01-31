using FluentValidation;
using Pension.Domain.Entities;
using Pension.Domain.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Pension.Application.Validators
{
    public class ContributionValidator : AbstractValidator<Contribution>
    {
        private readonly IContributionRepository _contributionRepository;

        public ContributionValidator(IContributionRepository contributionRepository)
        {
            _contributionRepository = contributionRepository;

            RuleFor(c => c.Type)
                .IsInEnum().WithMessage("Invalid contribution type. Must be Monthly or Voluntary.");

            RuleFor(c => c.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than zero.");

            RuleFor(c => c.ContributionDate)
                .MustAsync(async (contribution, date, cancellationToken) => await IsValidContributionDateAsync(contribution, cancellationToken))
                .WithMessage("Monthly contributions must be one per calendar month.");

            RuleFor(c => c.ReferenceNumber)
                .Matches("^[A-Za-z0-9]{8,12}$")
                .WithMessage("Reference number must be 8-12 alphanumeric characters.");
        }

        private async Task<bool> IsValidContributionDateAsync(Contribution contribution, CancellationToken cancellationToken)
        {
            if (contribution.Type == ContributionType.Voluntary)
                return true;

            // Ensure only one Monthly contribution exists per calendar month
            var existingContributions = await _contributionRepository.GetByMemberIdAsync(contribution.MemberId);

            return !existingContributions.Any(c => c.Type == ContributionType.Monthly &&
                                                   c.ContributionDate.Year == contribution.ContributionDate.Year &&
                                                   c.ContributionDate.Month == contribution.ContributionDate.Month);
        }
    }
}
