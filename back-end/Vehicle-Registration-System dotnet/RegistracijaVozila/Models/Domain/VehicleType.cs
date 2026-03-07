namespace VehicleRegistrationSystem.Models.Domain
{
    public class VehicleType
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public ICollection<VehicleBrand> Brands { get; set; } = new HashSet<VehicleBrand>();
    }
}