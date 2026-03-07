namespace VehicleRegistrationSystem.Models.Domain
{
    public class VehicleModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public Guid VehicleBrandId { get; set; }

        public VehicleBrand VehicleBrand { get; set; }
    }
}