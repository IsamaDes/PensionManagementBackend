// Pensions.Infrastructure/Repositories/EmployerRepository.cs
using Pension.Domain.Entities;
using Pension.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Pension.Infrastructure.Repositories
{
    public class EmployerRepository : IEmployerRepository
    {
        private readonly PensionsDbContext _context;

        public EmployerRepository(PensionsDbContext context)
        {
            _context = context;
        }

        public async Task<Employer?> GetByIdAsync(Guid id)
        {
            return await _context.Employers.FindAsync(id);
        }
    }
}