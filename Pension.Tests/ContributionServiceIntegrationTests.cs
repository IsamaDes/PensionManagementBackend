// using Xunit;
// using Moq;
// using System;
// using System.Threading.Tasks;
// using FluentAssertions;
// using Pension.Domain.Entities;
// using Pension.Domain.Services;
// using Pension.Domain.Repositories;
// using Pension.Infrastructure.Repositories;
// using Microsoft.EntityFrameworkCore;
// using Pension.Infrastructure;

// namespace Pension.Tests
// {
//     public class ContributionServiceIntegrationTests
//     {
//         private readonly PensionsDbContext _context;
//         private readonly ContributionRepository _contributionRepo;
//         private readonly ContributionService _service;

//         public ContributionServiceIntegrationTests()
//         {
//             var options = new DbContextOptionsBuilder<PensionsDbContext>()
//                 .UseInMemoryDatabase(databaseName: "PensionsDb")
//                 .Options;

//             _context = new PensionsDbContext(options);
//             _contributionRepo = new ContributionRepository(_context);

//             // Mock dependencies
//             var mockMemberRepo = new Mock<IMemberRepository>();
//             var mockEmployerRepo = new Mock<IEmployerRepository>();

//             _service = new ContributionService(mockMemberRepo.Object, mockEmployerRepo.Object, _contributionRepo);
//         }

//         [Fact]
//         public async Task AddContributionAsync_ShouldSaveContributionInDb()
//         {
//             // Arrange
//             var memberId = Guid.NewGuid();
//             var employerId = Guid.NewGuid();
//             var amount = 100m;
//             bool isMandatory = true;

//             // Act
//             await _service.AddContributionAsync(memberId, employerId, amount, isMandatory);

//             // Assert
//             var contributions = await _context.Contributions.ToListAsync();
//             contributions.Should().NotBeEmpty(); // FluentAssertions
//         }
//     }
// }
