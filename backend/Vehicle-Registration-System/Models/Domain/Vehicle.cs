namespace VehicleRegistrationSystem.Models.Domain
{
    public class Vehicle
    {
        public Guid Id { get; set; }

        public Guid VehicleTypeId { get; set; }
        public VehicleType VehicleType { get; set; }

        public Guid VehicleBrandId { get; set; }
        public VehicleBrand VehicleBrand { get; set; }

        public Guid VehicleModelId { get; set; }
        public VehicleModel VehicleModel { get; set; }

        public int ProductionYear { get; set; }

        public float EngineCapacity { get; set; }

        public string FuelType { get; set; } = string.Empty;

        public float Weight { get; set; }

        public int EnginePowerKw { get; set; }

        public string ChassisNumber { get; set; } = string.Empty;

        public DateTime FirstRegistrationDate { get; set; }

        public Registration Registration { get; set; }
    }
}