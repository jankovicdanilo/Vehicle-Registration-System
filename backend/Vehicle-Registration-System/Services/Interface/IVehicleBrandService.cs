using VehicleRegistrationSystem.Models.DTO.VehicleBrand;
using VehicleRegistrationSystem.Results;

namespace VehicleRegistrationSystem.Services.Interface
{
    public interface IVehicleBrandService
    {
        Task<Result<bool>> ValidateVehicleBrandCreateRequestAsync(CreateVehicleBrandRequestDto request);

        Task<Result<bool>> ValidateVehicleBrandDeleteRequestAsync(Guid id);

        Task<Result<bool>> ValidateVehicleBrandGetListByTypeAsync(Guid id);

        Task<Result<bool>> ValidateVehicleBrandGetByIdAsync(Guid id);

        Task<Result<bool>> ValidateVehicleBrandUpdateRequestAsync(UpdateVehicleBrandRequestDto request);

        Task<Result<VehicleBrandDto>> CreateVehicleBrand(CreateVehicleBrandRequestDto request);

        Task<Result<VehicleBrandDto>> DeleteVehicleBrand(Guid id);

        Task<Result<VehicleBrandDto>> UpdateVehicleBrand(UpdateVehicleBrandRequestDto request);

        Task<Result<List<VehicleBrandDto>>> GetListByType(Guid id);

        Task<Result<VehicleBrandDto>> GetById(Guid id);
    }
}
