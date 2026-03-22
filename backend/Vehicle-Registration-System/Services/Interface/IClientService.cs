using VehicleRegistrationSystem.Models.DTO.Client;
using VehicleRegistrationSystem.Results;

namespace VehicleRegistrationSystem.Services.Interface
{
    public interface IClientService
    {
        Task<RepositoryResult<bool>> ValidateClientCreateRequestAsync(CreateClientRequestDto request);

        Task<RepositoryResult<bool>> ValidateClientDeleteRequestAsync(Guid id);

        Task<RepositoryResult<bool>> ValidateClientUpdateRequestAsync(UpdateClientRequestDto request);

        Task<RepositoryResult<bool>> ValidateClientId(Guid id);

        Task<RepositoryResult<ClientDto>> CreateClientAsync(CreateClientRequestDto request);

        Task<RepositoryResult<ClientDto>> DeleteClientAsync(Guid id);

        Task<RepositoryResult<ClientDto>> UpdateClientAsync(UpdateClientRequestDto request);

        Task<RepositoryResult<PagedResult<ClientListItemDto>>> GetClientsAsync
            (string? searchQuery = null, int pageNumber = 1, int pageSize = 10);

        Task<RepositoryResult<ClientDto>> GetClientByIdAsync(Guid id);

        
    }
}
