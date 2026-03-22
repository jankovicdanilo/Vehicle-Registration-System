using VehicleRegistrationSystem.Models.DTO.Vehicle;
using VehicleRegistrationSystem.Results;

namespace VehicleRegistrationSystem.Services.Interface
{
    public interface IVehicleService
    {
        Task<RepositoryResult<bool>> ValidateVehicleCreateRequestAsync(CreateVehicleRequestDto request);

        Task<RepositoryResult<bool>?> ValidateVehicleDeleteRequestAsync(Guid id);

        Task<RepositoryResult<bool>?> ValidateGetVehicleByIdRequestAsync(Guid id);

        Task<RepositoryResult<bool>?> ValidateVehicleUpdateRequestAsync(UpdateVehicleDto request);

        Task<RepositoryResult<VehicleDto>> CreateVehicleAsync(CreateVehicleRequestDto request);

        Task<RepositoryResult<VehicleDto>> DeleteVehicleAsync(Guid id);

        Task<RepositoryResult<VehicleDto>> UpdateVehicleAsync(UpdateVehicleDto request);

        Task<RepositoryResult<PagedResult<VehicleListItemDto>>>  GetAllAsync
            (string? searchQuery = null, int pageSize = 10, int pageNumber = 1);

        Task<RepositoryResult<VehicleDto>> GetVehicleByIdAsync(Guid id);
    }
}
