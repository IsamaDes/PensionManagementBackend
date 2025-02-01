namespace Pension.Domain.Entities
{
    public enum ContributionType
    {
        Monthly,
        Voluntary
    }

    public class Contribution
    {
        public Guid Id { get; set; }
        public ContributionType Type { get; set; }  // Keep only one property for the contribution type
        public decimal Amount { get; set; }
        public DateTime ContributionDate { get; set; }
        public string ReferenceNumber { get; set; } = string.Empty;
        public Guid MemberId { get; set; }
        public virtual Member Member { get; set; } = null!;

        // Constructor
        public Contribution(Guid id, ContributionType type, decimal amount, DateTime contributionDate, string referenceNumber, Guid memberId)
        {
            Id = id;
            Type = type;
            Amount = amount;
            ContributionDate = contributionDate;
            ReferenceNumber = referenceNumber;
            MemberId = memberId;
        }
    }
}
