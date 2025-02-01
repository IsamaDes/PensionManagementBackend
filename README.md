# Pension Management System

This repository contains the code for a Pension Management System built with Clean Architecture, Domain-Driven Design (DDD), and .NET technologies. The system handles pension contributions, member data, and related business logic.

## Project Structure

The solution is organized into several projects:

PensionManagementBackend/
├── PensionManagementBackend.sln
├── Pension.API/
│ ├── Controllers/
│ ├── Middleware/
│ └── Properties/
├── Pension.Infrastructure/
│ ├── Data/
│ ├── Repositories/
│ ├── Services/
│ ├── Migrations/
├── Pension.Domain/
│ ├── Aggregates/
│ ├── Entities/
│ ├── Exceptions/
│ ├── Repositories/
│ ├── Services/
│ ├── ValueObjects/
├── Pension.Application/
│ ├── DTOs/
│ ├── Validators/
│ ├── Services/
├── Pension.API.Tests/
├── Pension.Infrastructure.Tests/
├── Pension.Domain.Tests/
├── Pension.Application.Tests/
└── Shared/

## Key Projects and Folders

- **PensionsManagementApi**: Exposes HTTP endpoints for interacting with the pension management system.
- **Pensions.Domain**: Contains the domain model for the pension system, including entities (e.g., Member, Contribution), value objects (e.g., Money, Address), and domain services (e.g., ContributionService).
- **Pensions.Infrastructure**: Handles data access through Entity Framework Core, with the `PensionsDbContext` and database migrations.
- **PensionsManagementSystem.sln**: The solution file that includes all the projects in the system.

## Setup Instructions

1. Clone this repository:
   `git clone <repository_url>`

2. Navigate to the project folder and restore dependencies:
   `dotnet restore`
3. Build the solution:
   `dotnet build`
4. Apply database migrations (if applicable):
   `dotnet ef database update --project Pensions.Infrastructure --startup-project PensionsManagementApi`
5. Run the application:
   `dotnet run --project PensionsManagementApi`

Folder Structure Breakdown
Pensions.Domain
The Pensions.Domain folder contains the core logic and business rules. It is organized into:

Entities: Aggregate root entities such as Member, Contribution, Employer, Benefit, and TransactionHistory.
ValueObjects: Contains value objects like Address and Money.
Aggregates: Contains aggregate roots like Pension.
Repositories: Interfaces for repositories like IMemberRepository for data access.
Services: Domain services such as ContributionService that implement business logic.
Pensions.Infrastructure
This folder provides the implementation of repositories, database context, and migrations for data access.

PensionsDbContext.cs: Defines the database context for Entity Framework Core.
Migrations: Contains migration files that handle database schema changes.
