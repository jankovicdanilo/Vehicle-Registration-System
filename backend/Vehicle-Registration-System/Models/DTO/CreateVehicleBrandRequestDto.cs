namespace VehicleRegistrationSystem.Models.DTO
{
    public class CreateVehicleBrandRequestDto
    {
        public string Name { get; set; }

        public Guid VehicleTypeId { get; set; }
    }
}
