namespace VehicleRegistrationSystem.Models.DTO
{
    public class UpdateVehicleModelRequestDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid VehicleBrandId { get; set; }
        
    }
}
