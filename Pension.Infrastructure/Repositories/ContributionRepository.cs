using Pension.Domain.Entities;
using Pension.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;


namespace Pension.Infrastructure.Repositories
{

    public class ContributionRepository : IContributionRepository
    {
        private readonly PensionsDbContext _context;

        public ContributionRepository(PensionsDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Contribution contribution)
        {
            _context.Contributions.Add(contribution);
            await _context.SaveChangesAsync();
        }

        public async Task<Contribution?> GetByIdAsync(Guid id)
        {
            return await _context.Contributions.FindAsync(id);
        }

        public async Task<IEnumerable<Contribution>> GetByMemberIdAsync(Guid memberId)
        {
            return await _context.Contributions
                .Where(c => c.MemberId == memberId)
                .ToListAsync();
        }

        public async Task<Contribution?> GetByMemberAndMonthAsync(Guid memberId, int month, int year)
        {
            return await _context.Contributions
                .FirstOrDefaultAsync(c => c.MemberId == memberId && c.ContributionDate.Month == month && c.ContributionDate.Year == year && c.Type == ContributionType.Monthly);
        }

        public async Task UpdateAsync(Contribution contribution)
        {
            _context.Contributions.Update(contribution);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var contribution = await _context.Contributions.FindAsync(id);
            if (contribution != null)
            {
                _context.Contributions.Remove(contribution);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Contribution>> GetAllAsync()
        {
            return await _context.Contributions.ToListAsync();
        }
    }


}