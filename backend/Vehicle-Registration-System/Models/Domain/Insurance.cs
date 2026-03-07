namespace VehicleRegistrationSystem.Models.Domain
{
    public class Insurance
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public ICollection<Registration> Registrations { get; set; } = new HashSet<Registration>();

        public ICollection<InsurancePrice> InsurancePrices { get; set; } = new HashSet<InsurancePrice>();
    }
}