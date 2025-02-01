using Pension.Domain.Entities;
using Pension.Domain.Repositories;

namespace Pension.Domain.Services
{
    public class ContributionService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IEmployerRepository _employerRepository;
        private readonly IContributionRepository _contributionRepository;

        public ContributionService(
            IMemberRepository memberRepository,
            IEmployerRepository employerRepository,
            IContributionRepository contributionRepository)
        {
            _memberRepository = memberRepository;
            _employerRepository = employerRepository;
            _contributionRepository = contributionRepository;
        }

        public async Task AddContributionAsync(Guid memberId, Guid employerId, decimal amount, bool isMandatory)
        {
            var member = await _memberRepository.GetByIdAsync(memberId);
            if (member == null)
            {
                throw new ArgumentException("Member not found");
            }

            var employer = await _employerRepository.GetByIdAsync(employerId);
            if (employer == null)
            {
                throw new ArgumentException("Employer not found");
            }

            // Ensure only one mandatory contribution per month
            if (isMandatory)
            {
                var existingMandatoryContribution = await _contributionRepository.GetByMemberAndMonthAsync(memberId, DateTime.Now.Month, DateTime.Now.Year);
                if (existingMandatoryContribution != null)
                {
                    throw new InvalidOperationException("Only one mandatory contribution per month is allowed.");
                }
            }

            // Assuming 'referenceNumber' is generated or retrieved here
            string referenceNumber = "REF123456";

            // Create a new contribution
            var contribution = new Contribution(
                memberId,
                isMandatory ? ContributionType.Monthly : ContributionType.Voluntary,
                amount,
                DateTime.Now,
                referenceNumber,
                employerId
            );

            // Add the contribution to the repository
            await _contributionRepository.AddAsync(contribution);
        }
    }

}
