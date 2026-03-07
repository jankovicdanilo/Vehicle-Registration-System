namespace VehicleRegistrationSystem.Models.Domain
{
    public class VehicleBrand
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid VehicleTypeId { get; set; }

        public VehicleType VehicleType { get; set; }

        public ICollection<VehicleModel> Models { get; set; } = new HashSet<VehicleModel>();
    }
}