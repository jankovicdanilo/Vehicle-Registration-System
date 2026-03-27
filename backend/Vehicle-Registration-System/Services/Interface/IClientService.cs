using VehicleRegistrationSystem.Models.DTO.Client;
using VehicleRegistrationSystem.Results;

namespace VehicleRegistrationSystem.Services.Interface
{
    public interface IClientService
    {
        Task<Result<bool>> ValidateClientCreateRequestAsync(CreateClientRequestDto request);

        Task<Result<bool>> ValidateClientDeleteRequestAsync(Guid id);

        Task<Result<bool>> ValidateClientUpdateRequestAsync(UpdateClientRequestDto request);

        Task<Result<bool>> ValidateClientId(Guid id);

        Task<Result<ClientDto>> CreateClientAsync(CreateClientRequestDto request);

        Task<Result<ClientDto>> DeleteClientAsync(Guid id);

        Task<Result<ClientDto>> UpdateClientAsync(UpdateClientRequestDto request);

        Task<Result<PagedResult<ClientListItemDto>>> GetClientsAsync
            (string? searchQuery = null, int pageNumber = 1, int pageSize = 10);

        Task<Result<ClientDto>> GetClientByIdAsync(Guid id);

        
    }
}
