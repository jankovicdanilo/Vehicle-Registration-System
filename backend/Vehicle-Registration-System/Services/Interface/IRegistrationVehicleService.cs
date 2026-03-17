using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Models.DTO;
using VehicleRegistrationSystem.Results;

namespace VehicleRegistrationSystem.Services.Interface
{
    public interface IRegistrationVehicleService
    {
        Task<RepositoryResult<bool>> 
            ValidateRegistrationCreateRequestAsync(CreateRegistrationVehicleRequestDto request,
            Vehicle vehicle, InsurancePrice? insurancePrice);

        Task<RepositoryResult<bool>?> ValidateRegistrationDeleteRequestAsync(Guid id);

        Task<RepositoryResult<bool>?> ValidateGetByIdAsync(Guid id);

        Task<RepositoryResult<bool>?> 
            ValidateRegistrationUpdateRequestAsync(UpdateRegistrationVehicleRequestDto request);

        Task<RepositoryResult<RegistrationVehicleDto>> 
            CreateRegistrationAsync(CreateRegistrationVehicleRequestDto request);

        Task<RepositoryResult<RegistrationVehicleDto>> DeleteRegistrationAsync(Guid id);

        Task<RepositoryResult<RegistrationVehicleDto>> 
            UpdateRegistrationAsync(UpdateRegistrationVehicleRequestDto request);

        Task<RepositoryResult<PagedResult<RegistrationVehicleDto>>> GetAllAsync
            (string? searchQuery = null, int pageNumber = 1, int pageSize = 1000);

        Task<RepositoryResult<RegistrationVehicleDto>> GetByIdAsync(Guid id);

        Task<RepositoryResult<RegistrationVehicleDto>> GenerateConfirmation(Guid id);
    }
}
