namespace VehicleRegistrationSystem.Models.DTO.VehicleBrand
{
    public class CreateVehicleBrandRequestDto
    {
        public string Name { get; set; }

        public Guid VehicleTypeId { get; set; }
    }
}
