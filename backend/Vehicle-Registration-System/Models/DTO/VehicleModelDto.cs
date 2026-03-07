namespace VehicleRegistrationSystem.Models.DTO
{
    public class VehicleModelDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid VehicleBrandId { get; set; }

        public string VehicleBrandName { get; set; }

        public Guid VehicleTypeId { get; set; }

        public string VehicleTypeName { get; set; }
    }
}
