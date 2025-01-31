using Pension.Domain.Entities;
using System.Threading.Tasks;

namespace Pension.Domain.Repositories
{
    public interface IEmployerRepository
    {
        Task<Employer> GetByIdAsync(Guid id);
    }
}
