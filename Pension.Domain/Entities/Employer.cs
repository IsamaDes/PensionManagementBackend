namespace Pension.Domain.Entities
{
    public class Employer
    {
        public Guid Id { get; set; }
        public required string CompanyName { get; set; }
        public required string RegistrationNumber { get; set; }
        public bool IsActive { get; set; } = true;
    }
}