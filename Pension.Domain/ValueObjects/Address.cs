namespace Pension.Domain.ValueObjects
{
    public class Address
    {
        public required string Street { get; set; }  // Ensure setter is available
        public required string City { get; set; }    // Ensure setter is available
        public required string ZipCode { get; set; } // Ensure setter is available

        // Constructor to initialize Address with required fields
        public Address(string street, string city, string zipCode)
        {
            Street = street;
            City = city;
            ZipCode = zipCode;
        }
    }
}
