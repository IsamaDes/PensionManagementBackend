using Microsoft.EntityFrameworkCore;
using Pension.Domain.Entities;
using Pension.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pension.Infrastructure.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly PensionsDbContext _context;

        public MemberRepository(PensionsDbContext context)
        {
            _context = context;
        }

        public async Task<Member> GetByIdAsync(Guid id)
        {
            var member = await _context.Members
                .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);

            if (member == null)
            {
                throw new KeyNotFoundException($"Member with ID {id} not found.");
            }

            return member;
        }

        public async Task<IEnumerable<Member>> GetAllMembersAsync()
        {
            return await _context.Members
                .Where(m => !m.IsDeleted)
                .ToListAsync();
        }

        public async Task AddAsync(Member member)
        {
            _context.Members.Add(member);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Member member)
        {
            _context.Members.Update(member);
            await _context.SaveChangesAsync();
        }

        // Implement soft delete
        public async Task DeleteAsync(Guid id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member != null)
            {
                member.IsDeleted = true;  // Soft delete by setting IsDeleted flag
                _context.Members.Update(member);
                await _context.SaveChangesAsync();
            }
        }
    }
}
