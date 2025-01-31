using Pension.Domain.Entities;

namespace Pension.Domain.Repositories
{
    public interface IMemberRepository
    {
        Task<Member> GetByIdAsync(Guid id);
        Task<IEnumerable<Member>> GetAllMembersAsync();
        Task AddAsync(Member member);
        Task UpdateAsync(Member member);
        Task DeleteAsync(Guid id);
    }
}
