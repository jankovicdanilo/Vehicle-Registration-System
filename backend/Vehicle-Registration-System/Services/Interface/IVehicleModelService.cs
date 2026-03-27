using VehicleRegistrationSystem.Models.DTO.VehicleModel;
using VehicleRegistrationSystem.Results;

namespace VehicleRegistrationSystem.Services.Interface
{
    public interface IVehicleModelService
    {
        Task<Result<bool>> ValidateVehicleModelCreateRequestAsync(CreateVehicleModelRequestDto request);

        Task<Result<bool>?> ValidateVehicleModelDeleteRequestAsync(Guid id);

        Task<Result<bool>?> ValidateVehicleModelGetByIdAsync(Guid id);

        Task<Result<bool>?> ValidateVehicleModelGetByBrandIdAsync(Guid id);

        Task<Result<bool>?> ValidateVehicleModelUpdateRequestAsync(UpdateVehicleModelRequestDto request);

        Task<Result<VehicleModelDto>> CreateVehicleModelAsync(CreateVehicleModelRequestDto request);

        Task<Result<VehicleModelDto>> DeleteVehicleModelAsync(Guid id);

        Task<Result<VehicleModelDto>> UpdateVehicleModelAsync(UpdateVehicleModelRequestDto request);

        Task<Result<VehicleModelDto>> GetById(Guid id);

        Task<Result<List<VehicleModelDto>>> GetByBrandId(Guid id);
    }
}
