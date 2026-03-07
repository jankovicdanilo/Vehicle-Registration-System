using VehicleRegistrationSystem.Models.Domain;

namespace VehicleRegistrationSystem.Models.Domain
{
    public class Registration
    {
        public Guid Id { get; set; }

        public DateTime RegistrationDate { get; set; }

        public DateTime ExpirationDate { get; set; }

        public decimal RegistrationPrice { get; set; }

        public string LicensePlate { get; set; } = string.Empty;

        public bool IsTemporary { get; set; }

        public Guid ClientId { get; set; }

        public Client Client { get; set; }

        public Guid VehicleId { get; set; }

        public Vehicle Vehicle { get; set; }

        public Guid InsuranceId { get; set; }

        public Insurance Insurance { get; set; }
    }
}