using VehicleRegistrationSystem.Models.DTO.VehicleType;
using VehicleRegistrationSystem.Results;

namespace VehicleRegistrationSystem.Services.Interface
{
    public interface IVehicleTypeService
    {
        Task<Result<bool>> ValidateVehicleTypeCreateRequestAsync(CreateVehicleTypeRequestDto request);

        Task<Result<bool>> ValidateVehicleTypeDeleteRequestAsync(Guid id);

        Task<Result<bool>> ValidateVehicleTypeGetByIdAsync(Guid id);

        Task<Result<bool>> ValidateVehicleTypeUpdateRequestAsync(UpdateVehicleTypeRequestDto request);

        Task<Result<VehicleTypeDto>> CreateVehicleTypeAsync(CreateVehicleTypeRequestDto request);

        Task<Result<VehicleTypeDto>> DeleteVehicleTypeAsync(Guid id);

        Task<Result<VehicleTypeDto>> UpdateVehicleTypeAsync(UpdateVehicleTypeRequestDto request);

        Task<Result<List<VehicleTypeDto>>> GetAllAsync();

        Task<Result<VehicleTypeDto>> GetById(Guid id);
    }
}
