using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Models.DTO.Registration;
using VehicleRegistrationSystem.Results;

namespace VehicleRegistrationSystem.Services.Interface
{
    public interface IRegistrationVehicleService
    {
        Task<Result<bool>> 
            ValidateRegistrationCreateRequestAsync(CreateRegistrationVehicleRequestDto request,
            Vehicle vehicle, InsurancePrice? insurancePrice);

        Task<Result<bool>?> ValidateRegistrationDeleteRequestAsync(Guid id);

        Task<Result<bool>?> ValidateGetByIdAsync(Guid id);

        Task<Result<bool>?> 
            ValidateRegistrationUpdateRequestAsync(UpdateRegistrationVehicleRequestDto request);

        Task<Result<RegistrationVehicleDto>> 
            CreateRegistrationAsync(CreateRegistrationVehicleRequestDto request);

        Task<Result<RegistrationVehicleDto>> DeleteRegistrationAsync(Guid id);

        Task<Result<RegistrationVehicleDto>> 
            UpdateRegistrationAsync(UpdateRegistrationVehicleRequestDto request);

        Task<Result<PagedResult<RegistrationVehicleListItemDto>>> GetAllAsync
            (string? searchQuery = null, int pageNumber = 1, int pageSize = 10);

        Task<Result<RegistrationDetailsDto>> GetByIdAsync(Guid id);

        Task<Result<RegistrationVehicleDto>> GenerateConfirmation(Guid id);
    }
}
