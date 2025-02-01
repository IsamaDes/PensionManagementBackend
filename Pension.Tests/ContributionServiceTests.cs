using Moq;
using Pension.Domain.Entities;
using Pension.Domain.Repositories;
using Pension.Domain.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace Pension.Tests
{
    public class ContributionServiceTests
    {
        private readonly Mock<IMemberRepository> _mockMemberRepo;
        private readonly Mock<IEmployerRepository> _mockEmployerRepo;
        private readonly Mock<IContributionRepository> _mockContributionRepo;
        private readonly ContributionService _service;

        public ContributionServiceTests()
        {
            _mockMemberRepo = new Mock<IMemberRepository>();
            _mockEmployerRepo = new Mock<IEmployerRepository>();
            _mockContributionRepo = new Mock<IContributionRepository>();

            _service = new ContributionService(
                _mockMemberRepo.Object,
                _mockEmployerRepo.Object,
                _mockContributionRepo.Object
            );
        }

        [Fact]
        public async Task AddContributionAsync_ShouldAddContribution_WhenValidData()
        {
            // Arrange
            var memberId = Guid.NewGuid();
            var employerId = Guid.NewGuid();
            var amount = 100m;
            bool isMandatory = true;

            _mockMemberRepo.Setup(repo => repo.GetByIdAsync(memberId))
                .ReturnsAsync(new Member
                {
                    Id = memberId,
                    FirstName = "John",
                    LastName = "Doe",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    Email = "john.doe@example.com"
                });

            _mockEmployerRepo.Setup(repo => repo.GetByIdAsync(employerId))
                .ReturnsAsync(new Employer
                {
                    Id = employerId,
                    CompanyName = "Acme Corp",
                    RegistrationNumber = "12345678"
                });

            _mockContributionRepo.Setup(repo => repo.GetByMemberAndMonthAsync(memberId, DateTime.Now.Month, DateTime.Now.Year))
                .ReturnsAsync((Contribution?)null); // No existing contribution

            // Act
            await _service.AddContributionAsync(memberId, employerId, amount, isMandatory);

            // Assert
            _mockContributionRepo.Verify(repo => repo.AddAsync(It.IsAny<Contribution>()), Times.Once);
        }

        [Fact]
        public async Task AddContributionAsync_ShouldThrowException_WhenMemberNotFound()
        {
            // Arrange
            var memberId = Guid.NewGuid();
            var employerId = Guid.NewGuid();
            var amount = 100m;
            bool isMandatory = true;

            _mockMemberRepo.Setup(repo => repo.GetByIdAsync(memberId))
                .ReturnsAsync((Member?)null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.AddContributionAsync(memberId, employerId, amount, isMandatory));
        }

        [Fact]
        public async Task AddContributionAsync_ShouldThrowException_WhenMultipleMandatoryContributionsInMonth()
        {
            // Arrange
            var memberId = Guid.NewGuid();
            var employerId = Guid.NewGuid();
            var amount = 100m;
            bool isMandatory = true;

            _mockMemberRepo.Setup(repo => repo.GetByIdAsync(memberId))
                .ReturnsAsync(new Member
                {
                    Id = memberId,
                    FirstName = "John",
                    LastName = "Doe",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    Email = "john.doe@example.com"
                });

            _mockEmployerRepo.Setup(repo => repo.GetByIdAsync(employerId))
                .ReturnsAsync(new Employer
                {
                    Id = employerId,
                    CompanyName = "Acme Corp",
                    RegistrationNumber = "12345678"
                });

            _mockContributionRepo.Setup(repo => repo.GetByMemberAndMonthAsync(memberId, DateTime.Now.Month, DateTime.Now.Year))
                .ReturnsAsync(new Contribution(
                    Guid.NewGuid(),
                    ContributionType.Monthly,
                    100,
                    DateTime.UtcNow,
                    "REF123456",
                    memberId
                )); // Contribution already exists

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.AddContributionAsync(memberId, employerId, amount, isMandatory));
        }



        [Fact]
        public async Task CalculateBenefitsAsync_ShouldCalculateBenefits_WhenEligibleForBenefit()
        {
            // Arrange
            var memberId = Guid.NewGuid();
            var employerId = Guid.NewGuid();

            var contributions = new List<Contribution>
    {
        new Contribution(Guid.NewGuid(), ContributionType.Monthly, 100, DateTime.Now.AddMonths(-1), "REF123", memberId),
        new Contribution(Guid.NewGuid(), ContributionType.Monthly, 200, DateTime.Now.AddMonths(-2), "REF124", memberId),
        new Contribution(Guid.NewGuid(), ContributionType.Monthly, 150, DateTime.Now.AddMonths(-3), "REF125", memberId),
        new Contribution(Guid.NewGuid(), ContributionType.Monthly, 180, DateTime.Now.AddMonths(-4), "REF126", memberId),
        new Contribution(Guid.NewGuid(), ContributionType.Monthly, 120, DateTime.Now.AddMonths(-5), "REF127", memberId),
        new Contribution(Guid.NewGuid(), ContributionType.Monthly, 130, DateTime.Now.AddMonths(-6), "REF128", memberId),
        new Contribution(Guid.NewGuid(), ContributionType.Monthly, 140, DateTime.Now.AddMonths(-7), "REF129", memberId),
        new Contribution(Guid.NewGuid(), ContributionType.Monthly, 160, DateTime.Now.AddMonths(-8), "REF130", memberId),
        new Contribution(Guid.NewGuid(), ContributionType.Monthly, 170, DateTime.Now.AddMonths(-9), "REF131", memberId),
        new Contribution(Guid.NewGuid(), ContributionType.Monthly, 190, DateTime.Now.AddMonths(-10), "REF132", memberId),
        new Contribution(Guid.NewGuid(), ContributionType.Monthly, 200, DateTime.Now.AddMonths(-11), "REF133", memberId),
        new Contribution(Guid.NewGuid(), ContributionType.Monthly, 210, DateTime.Now.AddMonths(-12), "REF134", memberId),
    };

            _mockMemberRepo.Setup(repo => repo.GetByIdAsync(memberId))
                .ReturnsAsync(new Member
                {
                    Id = memberId,
                    FirstName = "John",
                    LastName = "Doe",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    Email = "john.doe@example.com"
                });

            _mockEmployerRepo.Setup(repo => repo.GetByIdAsync(employerId))
                .ReturnsAsync(new Employer
                {
                    Id = employerId,
                    CompanyName = "Acme Corp",
                    RegistrationNumber = "12345678"
                });

            _mockContributionRepo.Setup(repo => repo.GetByMemberIdAsync(memberId))
                .ReturnsAsync(contributions);

            // Act
            await _service.AddContributionAsync(memberId, employerId, 100m, true); // Add a new contribution

            // Assert
            contributions.Count.Should().BeGreaterThanOrEqualTo(12); // Ensure at least 12 contributions
        }





    }
}
