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


        public async Task AddContributionAsync(Guid memberId, Guid employerId, decimal amount)
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

            // Assuming 'referenceNumber' is generated or retrieved here
            string referenceNumber = "REF123456";

            var contribution = new Contribution(
                memberId,
                ContributionType.Monthly,
                amount,
                DateTime.Now,
                referenceNumber,
                memberId
            );

            await _contributionRepository.AddAsync(contribution);
        }
    }
}
