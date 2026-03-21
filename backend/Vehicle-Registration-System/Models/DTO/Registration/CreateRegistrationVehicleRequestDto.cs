namespace VehicleRegistrationSystem.Models.DTO.Registration
{
    public class CreateRegistrationVehicleRequestDto
    {
        public string LicensePlate { get; set; }

        public DateTime RegistrationDate { get; set; }

        public bool IsTemporary { get; set; }

        public Guid ClientId { get; set; }

        public Guid VehicleId { get; set; }

        public Guid InsuranceId { get; set; }

    }
}
