namespace VehicleRegistrationSystem.Models.DTO.VehicleModel
{
    public class CreateVehicleModelRequestDto
    {
        public string Name { get; set; }

        public Guid VehicleBrandId { get; set; }
    }
}
