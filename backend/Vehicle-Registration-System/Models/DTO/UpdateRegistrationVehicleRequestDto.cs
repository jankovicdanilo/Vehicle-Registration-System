
namespace VehicleRegistrationSystem.Models.DTO
{
    public class UpdateRegistrationVehicleRequestDto
    {
        public Guid Id { get; set; }

        public string LicensePlate { get; set; }

        public DateTime RegistrationDate { get; set; }

        public bool IsTemporary { get; set; }

        public Guid ClientId { get; set; }

        public Guid VehicleId { get; set; }

        public Guid InsuranceId { get; set; }
    }
}
