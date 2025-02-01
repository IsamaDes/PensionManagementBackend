using Microsoft.EntityFrameworkCore;
using Moq;
using Pension.Infrastructure.Repositories;
using Pension.Infrastructure;
using Pension.Domain.Entities;
using Pension.Domain.Services;
using Pension.Domain.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace Pension.Tests
{
    public class ContributionServiceIntegrationTests
    {
        private readonly PensionsDbContext _context;
        private readonly ContributionRepository _contributionRepo;
        private readonly ContributionService _service;

        public ContributionServiceIntegrationTests()
        {
            var options = new DbContextOptionsBuilder<PensionsDbContext>()
                .UseInMemoryDatabase("PensionsDb")
                .Options;

            _context = new PensionsDbContext(options);
            _contributionRepo = new ContributionRepository(_context);

            // Create and initialize Member and Employer objects with required fields
            var member = new Member
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = DateTime.Now.AddYears(-30),
                Email = "john.doe@example.com"
            };

            var employer = new Employer
            {
                Id = Guid.NewGuid(),
                CompanyName = "Acme Corp",
                RegistrationNumber = "AC12345"
            };

            // Add them to the in-memory database
            _context.Members.Add(member);
            _context.Employers.Add(employer);
            _context.SaveChanges();

            // Mock the repositories
            var mockMemberRepo = new Mock<IMemberRepository>();
            var mockEmployerRepo = new Mock<IEmployerRepository>();

            mockMemberRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(member);
            mockEmployerRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(employer);

            // Pass mocked repositories to the service
            _service = new ContributionService(mockMemberRepo.Object, mockEmployerRepo.Object, _contributionRepo);
        }

        [Fact]
        public async Task AddContributionAsync_ShouldSaveContributionInDb()
        {
            // Arrange
            var memberId = _context.Members.First().Id;
            var employerId = _context.Employers.First().Id;
            var amount = 100m;
            bool isMandatory = true;

            // Act
            await _service.AddContributionAsync(memberId, employerId, amount, isMandatory);

            // Assert
            var contributions = await _context.Contributions.ToListAsync();
            contributions.Should().NotBeEmpty();
        }
    }
}
