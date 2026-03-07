namespace VehicleRegistrationSystem.Models.DTO
{
    public class UpdateVehicleTypeRequestDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }
    }
}
