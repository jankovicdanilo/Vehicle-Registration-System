
using VehicleRegistrationSystem.Models.DTO;

namespace VehicleRegistrationSystem.Models.DTO
{
    public class RegistrationVehicleDto
    {
        public Guid Id { get; set; }

        public string LicensePlate { get; set; }

        public DateTime RegistrationDate { get; set; }

        public DateTime ExpirationDate { get; set; }

        public decimal RegistrationPrice { get; set; }

        public bool IsTemporary { get; set; }

        public Guid ClientId { get; set; }

        public ClientDto Client { get; set; }

        public Guid VehicleId { get; set; }

        public VehicleDto Vehicle { get; set; }

        public Guid InsuranceId { get; set; }

        public InsuranceDto Insurance { get; set; }
    }
}
