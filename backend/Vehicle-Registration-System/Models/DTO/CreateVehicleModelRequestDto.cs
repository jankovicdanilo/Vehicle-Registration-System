namespace VehicleRegistrationSystem.Models.DTO
{
    public class CreateVehicleModelRequestDto
    {
        public string Name { get; set; }

        public Guid VehicleBrandId { get; set; }
    }
}
