using System.ComponentModel.DataAnnotations;

namespace VehicleRegistrationSystem.Models.DTO
{
    public class UpdateVehicleDto
    {
        [Required(ErrorMessage = "Id is missing")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "VehicleTypeId is missing")]
        public Guid VehicleTypeId { get; set; }

        [Required(ErrorMessage = "VehicleBrandId is missing")]
        public Guid VehicleBrandId { get; set; }

        [Required(ErrorMessage = "VehicleModelId is missing")]
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
