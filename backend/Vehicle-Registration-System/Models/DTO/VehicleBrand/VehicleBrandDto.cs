namespace VehicleRegistrationSystem.Models.DTO.VehicleBrand
{
    public class VehicleBrandDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid VehicleTypeId { get; set; }

        public string VehicleType { get; set; }

        public string Category { get; set; }
    }
}
