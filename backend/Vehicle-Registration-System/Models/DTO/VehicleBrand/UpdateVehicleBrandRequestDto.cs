namespace VehicleRegistrationSystem.Models.DTO.VehicleBrand
{
    public class UpdateVehicleBrandRequestDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid VehicleTypeId { get; set; }
    }
}
