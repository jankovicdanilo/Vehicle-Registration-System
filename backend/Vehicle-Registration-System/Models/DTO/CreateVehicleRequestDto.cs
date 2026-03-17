using System.ComponentModel.DataAnnotations;

namespace VehicleRegistrationSystem.Models.DTO
{
    public class CreateVehicleRequestDto
    {
        public Guid VehicleTypeId { get; set; }

        public Guid VehicleBrandId { get; set; }

        public Guid VehicleModelId { get; set; }

        public int ProductionYear { get; set; }

        public float EngineCapacity { get; set; }

        public string FuelType { get; set; }

        public float Weight { get; set; }

        public int EnginePowerKw { get; set; }

        public string ChassisNumber { get; set; }

        public DateTime FirstRegistrationDate { get; set; }
    }
}




