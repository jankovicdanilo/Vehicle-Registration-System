using VehicleRegistrationSystem.Models.DTO.Vehicle;
using VehicleRegistrationSystem.Results;

namespace VehicleRegistrationSystem.Services.Interface
{
    public interface IVehicleService
    {
        Task<Result<bool>> ValidateVehicleCreateRequestAsync(CreateVehicleRequestDto request);

        Task<Result<bool>?> ValidateVehicleDeleteRequestAsync(Guid id);

        Task<Result<bool>?> ValidateGetVehicleByIdRequestAsync(Guid id);

        Task<Result<bool>?> ValidateVehicleUpdateRequestAsync(UpdateVehicleDto request);

        Task<Result<VehicleDto>> CreateVehicleAsync(CreateVehicleRequestDto request);

        Task<Result<VehicleDto>> DeleteVehicleAsync(Guid id);

        Task<Result<VehicleDto>> UpdateVehicleAsync(UpdateVehicleDto request);

        Task<Result<PagedResult<VehicleListItemDto>>>  GetAllAsync
            (string? searchQuery = null, int pageSize = 10, int pageNumber = 1);

        Task<Result<VehicleDto>> GetVehicleByIdAsync(Guid id);
    }
}
