using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Models.DTO;
using VehicleRegistrationSystem.Repositories.Interface;
using VehicleRegistrationSystem.Results;
using VehicleRegistrationSystem.Services.Interface;
using VehicleRegistrationSystem.Data;
using VehicleRegistrationSystem.Models.DTO;

namespace VehicleRegistrationSystem.Services.Implementation
{
    public class ClientService : IClientService
    {
        private readonly VehicleRegistrationDbContext appDbContext;
        private readonly IMapper mapper;
        private readonly IClientRepository clientRepository;

        public ClientService(VehicleRegistrationDbContext appDbContext, 
            IMapper mapper, IClientRepository clientRepository)
        {
            this.appDbContext = appDbContext;
            this.mapper = mapper;
            this.clientRepository = clientRepository;
        }

        public async Task<RepositoryResult<bool>> ValidateClientCreateRequestAsync(CreateClientRequestDto request)
        {
            if (await appDbContext.Clients.AnyAsync(x => x.NationalId == request.NationalId))
            {
                return RepositoryResult<bool>.Fail
                    ("JMBG_EXISTS: A client with this social security number already exists");
            }

            if (await appDbContext.Clients.AnyAsync(x => x.IdCardNumber == request.IdCardNumber))
            {
                return RepositoryResult<bool>.Fail
                    ("ID_CARD_EXISTS: A client with this ID card number already exists");
            }

            if (await appDbContext.Clients.AnyAsync(x => x.Email == request.Email))
            {
                return RepositoryResult<bool>.Fail
                    ("EMAIL_EXISTS: A client with this email adress already exists");
            }

            return RepositoryResult<bool>.Ok(true);
        }

        public async Task<RepositoryResult<ClientDto>> CreateClientAsync(CreateClientRequestDto request)
        {
            var validationResult = await ValidateClientCreateRequestAsync(request);

            if (!validationResult.Success)
            {
                return RepositoryResult<ClientDto>.Fail(validationResult.Message);
            }

            var clientDomain = mapper.Map<Client>(request);

            clientDomain = await clientRepository.AddClientAsync(clientDomain);

            var response = mapper.Map<ClientDto>(clientDomain);

            return RepositoryResult<ClientDto>.Ok(response, "Client successfully created!");
        }

        public async Task<RepositoryResult<bool>> ValidateClientDeleteRequestAsync(Guid id)
        {
            var exists = await appDbContext.Clients.AnyAsync(x=>x.Id == id);

            if(!exists)
            {
                return RepositoryResult<bool>.Fail($"CLIENT_NOT_FOUND: Client with the Id {id} was not found");
            }

            var isOwner = await appDbContext.Registrations.AnyAsync(x=>x.ClientId == id);

            if (isOwner)
            {
                return RepositoryResult<bool>.Fail
                    ("CLIENT_REGISTRATION_EXISTS: Client can't be deleted because his registration still exists");
            }

            return RepositoryResult<bool>.Ok(true);
        }

        public async Task<RepositoryResult<ClientDto>> DeleteClientAsync(Guid id)
        {
            var validationResult = await ValidateClientDeleteRequestAsync(id);

            if (!validationResult.Success)
            {
                return RepositoryResult<ClientDto>.Fail(validationResult.Message);
            }

            var existingClient = await clientRepository.DeleteClientAsync(id);

            var response = mapper.Map<ClientDto>(existingClient);

            return RepositoryResult<ClientDto>.Ok(response, "Client successfully deleted!");
        }

        public async Task<RepositoryResult<bool>> ValidateClientUpdateRequestAsync(UpdateClientRequestDto request)
        {
            var exists = await appDbContext.Clients.AnyAsync(x => x.Id == request.Id);

            if (!exists)
            {
                return RepositoryResult<bool>.Fail
                    ($"CLIENT_NOT_FOUND: Client with the Id {request.Id} was not found");
            }

            if (await appDbContext.Clients.AnyAsync(x => x.NationalId == request.NationalId && x.Id!=request.Id))
            {
                return RepositoryResult<bool>.Fail
                    ("NationalId_EXISTS: A client with this social security number already exists");
            }

            if (await appDbContext.Clients.AnyAsync
                (x => x.IdCardNumber == request.IdCardNumber && x.Id != request.Id))
            {
                return RepositoryResult<bool>.Fail
                    ("ID_CARD_EXISTS: A client with this ID card number already exists");
            }

            if (await appDbContext.Clients.AnyAsync(x => x.Email == request.Email && x.Id != request.Id))
            {
                return RepositoryResult<bool>.Fail
                    ("EMAIL_EXISTS: A client with this email adress already exists");
            }

            return RepositoryResult<bool>.Ok(true);
        }

        public async Task<RepositoryResult<ClientDto>> UpdateClientAsync(UpdateClientRequestDto request)
        {
            var validationResult = await ValidateClientUpdateRequestAsync(request);

            if (!validationResult.Success)
            {
                return RepositoryResult<ClientDto>.Fail(validationResult.Message);
            }

            var clientDomain = mapper.Map<Client>(request);

            clientDomain = await clientRepository.UpdateClientAsync(clientDomain);

            var response = mapper.Map<ClientDto>(clientDomain);

            return RepositoryResult<ClientDto>.Ok(response, "Client successfully updated!");
        }

        public async Task<RepositoryResult<PagedResult<ClientDto>>> GetClientsAsync
            (string? searchQuery = null, int pageNumber = 1, int pageSize = 1000)
        {
            var (clients, totalCount) = await clientRepository.GetAllAsync(searchQuery, pageNumber, pageSize);

            var response = new PagedResult<ClientDto>
            {
                Items = mapper.Map<List<ClientDto>>(clients),
                TotalCount = totalCount
            };


            return RepositoryResult<PagedResult<ClientDto>>.Ok(response, "Clients loaded successfully");
        }

        public async Task<RepositoryResult<bool>> ValidateClientId(Guid id)
        {
            if (!await appDbContext.Clients.AnyAsync(x => x.Id == id))
            {
                return RepositoryResult<bool>.Fail($"CLIENT_NOT_FOUND, Client with the Id {id} was not found");
            }

            return RepositoryResult<bool>.Ok(true);
        }

        public async Task<RepositoryResult<ClientDto>> GetClientByIdAsync(Guid id)
        {
            var validationResult = await ValidateClientId(id);

            if (!validationResult.Success)
            {
                return RepositoryResult<ClientDto>.Fail(validationResult.Message);
            }

            var clientDomain = await clientRepository.GetClientByIdAsync(id);

            var response = mapper.Map<ClientDto>(clientDomain);

            return RepositoryResult<ClientDto>.Ok(response);
        }

        
    }
}






