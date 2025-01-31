using Microsoft.EntityFrameworkCore;
using Pension.Domain.Entities;

namespace Pension.Infrastructure
{
    public class PensionsDbContext : DbContext
    {
        public PensionsDbContext(DbContextOptions<PensionsDbContext> options) : base(options) { }

        public DbSet<Member> Members { get; set; }
        public DbSet<Contribution> Contributions { get; set; }
        public DbSet<Employer> Employers { get; set; }
        public DbSet<Benefit> Benefits { get; set; }
        public DbSet<TransactionHistory> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contribution>()
            .Property(c => c.MemberId)
            .HasColumnType("uniqueidentifier");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PensionsDbContext).Assembly);
        }
    }
}
