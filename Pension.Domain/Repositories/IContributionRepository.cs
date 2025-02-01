using System.Collections.Generic;
using System.Threading.Tasks;
using Pension.Domain.Entities;  // Ensure the correct namespace for your entities

namespace Pension.Domain.Repositories
{
    public interface IContributionRepository
    {
        // Add a new contribution
        Task AddAsync(Contribution contribution);

        // Get a contribution by its ID
        Task<Contribution> GetByIdAsync(Guid id);

        // Get all contributions for a particular member or employer
        Task<IEnumerable<Contribution>> GetByMemberIdAsync(Guid memberId);

        // Update an existing contribution
        Task UpdateAsync(Contribution contribution);

        // Delete a contribution by its ID
        Task DeleteAsync(Guid id);

        // Get all contributions (you can add filtering or pagination here)
        Task<IEnumerable<Contribution>> GetAllAsync();

        // Get contributions by month and year
        Task<Contribution?> GetByMemberAndMonthAsync(Guid memberId, int month, int year);

    }
}
