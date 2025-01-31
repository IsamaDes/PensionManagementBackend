namespace Pension.Application.Dtos
{
    public class MemberDto
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required DateTime DateOfBirth { get; set; }
        public required string Email { get; set; }
    }
}
