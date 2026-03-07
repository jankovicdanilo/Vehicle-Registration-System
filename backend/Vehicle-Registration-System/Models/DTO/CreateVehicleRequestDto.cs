using System.ComponentModel.DataAnnotations;

namespace VehicleRegistrationSystem.Models.DTO
{
    public class CreateVehicleRequestDto
    {
        [Required(ErrorMessage = "VehicleTypeId field is required")]
        public Guid? VehicleTypeId { get; set; }

        [Required(ErrorMessage = "VehicleBrandId field is required")]
        public Guid? VehicleBrandId { get; set; }

        [Required(ErrorMessage = "VehicleModelId field is required")]
        public Guid? VehicleModelId { get; set; }

        public int ProductionYear { get; set; }

        public float EngineCapacity { get; set; }

        public string FuelType { get; set; }

        public float Weight { get; set; }

        public int EnginePowerKw { get; set; }

        public string ChassisNumber { get; set; }

        public DateTime FirstRegistrationDate { get; set; }
    }
}




