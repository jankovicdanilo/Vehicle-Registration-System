namespace VehicleRegistrationSystem.Models.DTO.Vehicle
{
    public class VehicleListItemDto
    {
        public Guid Id { get; set; }

        public int ProductionYear { get; set; }

        public decimal EngineCapacity { get; set; }

        public int EnginePowerKw { get; set; }

        public string ChassisNumber { get; set; }

        public string VehicleTypeName { get; set; }

        public string VehicleBrandName { get; set; }

        public string VehicleModelName { get; set; }
    }
}
