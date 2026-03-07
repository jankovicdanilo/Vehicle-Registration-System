using VehicleRegistrationSystem.Models.DTO;
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

        Task<RepositoryResult<PagedResult<VehicleDto>>>  GetAllAsync
            (string? searchQuery = null, int pageSize = 1000, int pageNumber = 1);

        Task<RepositoryResult<VehicleDto>> GetVehicleByIdAsync(Guid id);
    }
}
