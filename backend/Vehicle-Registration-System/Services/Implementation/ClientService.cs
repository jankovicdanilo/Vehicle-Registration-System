using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Repositories.Interface;
using VehicleRegistrationSystem.Results;
using VehicleRegistrationSystem.Services.Interface;
using VehicleRegistrationSystem.Data;
using VehicleRegistrationSystem.Repositories.Implementation;
using VehicleRegistrationSystem.Models.DTO.Client;

namespace VehicleRegistrationSystem.Services.Implementation
{
    public class ClientService : IClientService
    {
        private readonly IMapper mapper;
        private readonly IClientRepository clientRepository;
        private readonly ILogger<ClientService> logger;
        private readonly IRegistrationVehicleRepository registrationVehicleRepository;

        public ClientService(IMapper mapper, IClientRepository clientRepository,
            ILogger<ClientService> logger, IRegistrationVehicleRepository registrationVehicleRepository)
        {
            this.mapper = mapper;
            this.clientRepository = clientRepository;
            this.logger = logger;
            this.registrationVehicleRepository = registrationVehicleRepository;
        }

        public async Task<RepositoryResult<bool>> ValidateClientCreateRequestAsync(CreateClientRequestDto request)
        {
            if (await clientRepository.ExistsAsync(x => x.NationalId == request.NationalId))
            {
                logger.LogWarning
                    ("Client creation failed: NationalId {NationalId} already exists", request.NationalId);

                return RepositoryResult<bool>.Fail
                    ("JMBG_EXISTS: A client with this social security number already exists");
            }

            if (await clientRepository.ExistsAsync(x => x.IdCardNumber == request.IdCardNumber))
            {
                logger.LogWarning
                    ("Client creation failed: IdCardNumber {IdCardNumber} already exists", request.IdCardNumber);

                return RepositoryResult<bool>.Fail
                    ("ID_CARD_EXISTS: A client with this ID card number already exists");
            }

            if (await clientRepository.ExistsAsync(x => x.Email == request.Email))
            {
                logger.LogWarning("Client creation failed: Email {Email} already exists", request.Email);

                return RepositoryResult<bool>.Fail
                    ("EMAIL_EXISTS: A client with this email adress already exists");
            }

            return RepositoryResult<bool>.Ok(true);
        }

        public async Task<RepositoryResult<ClientDto>> CreateClientAsync(CreateClientRequestDto request)
        {
            logger.LogInformation("Starting client creation for email {Email}", request.Email);

            var validationResult = await ValidateClientCreateRequestAsync(request);

            if (!validationResult.Success)
            {
                logger.LogWarning("Client creation validation failed for email {Email}: {Message}",
                    request.Email, validationResult.Message);

                return RepositoryResult<ClientDto>.Fail(validationResult.Message);
            }

            var clientDomain = mapper.Map<Client>(request);

            clientDomain = await clientRepository.AddAsync(clientDomain);

            var response = mapper.Map<ClientDto>(clientDomain);

            logger.LogInformation("Client successfully created with ID {ClientId}", response.Id);

            return RepositoryResult<ClientDto>.Ok(response, "Client successfully created!");
        }

        public async Task<RepositoryResult<bool>> ValidateClientDeleteRequestAsync(Guid id)
        {
            var exists = await clientRepository.ExistsAsync(x => x.Id == id);

            if(!exists)
            {
                return RepositoryResult<bool>.Fail($"CLIENT_NOT_FOUND: Client with the Id {id} was not found");
            }

            var isOwner = await registrationVehicleRepository.ExistsAsync(x => x.ClientId == id);

            if (isOwner)
            {
                return RepositoryResult<bool>.Fail
                    ("CLIENT_REGISTRATION_EXISTS: Client can't be deleted because vehicles are " +
                    "still registered under this client.");
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

            var existingClient = await clientRepository.DeleteAsync(id);

            var response = mapper.Map<ClientDto>(existingClient);

            return RepositoryResult<ClientDto>.Ok(response, "Client successfully deleted!");
        }

        public async Task<RepositoryResult<bool>> ValidateClientUpdateRequestAsync(UpdateClientRequestDto request)
        {
            var exists = await clientRepository.ExistsAsync(x => x.Id == request.Id);

            if (!exists)
            {
                return RepositoryResult<bool>.Fail
                    ($"CLIENT_NOT_FOUND: Client with the Id {request.Id} was not found");
            }

            if (await clientRepository.ExistsAsync(x => x.NationalId == request.NationalId && x.Id!=request.Id))
            {
                return RepositoryResult<bool>.Fail
                    ("NationalId_EXISTS: A client with this social security number already exists");
            }

            if (await clientRepository.ExistsAsync
                (x => x.IdCardNumber == request.IdCardNumber && x.Id != request.Id))
            {
                return RepositoryResult<bool>.Fail
                    ("ID_CARD_EXISTS: A client with this ID card number already exists");
            }

            if (await clientRepository.ExistsAsync(x => x.Email == request.Email && x.Id != request.Id))
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

        public async Task<RepositoryResult<PagedResult<ClientListItemDto>>> GetClientsAsync
            (string? searchQuery = null, int pageNumber = 1, int pageSize = 10)
        {
            var (clients, totalCount) = await clientRepository.GetAllAsync(searchQuery, pageNumber, pageSize);

            var response = new PagedResult<ClientListItemDto>
            {
                Items = clients,
                TotalCount = totalCount
            };


            return RepositoryResult<PagedResult<ClientListItemDto>>.Ok(response, "Clients loaded successfully");
        }

        public async Task<RepositoryResult<bool>> ValidateClientId(Guid id)
        {
            if (!await clientRepository.ExistsAsync(x => x.Id == id))
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

            var clientDomain = await clientRepository.GetByIdAsync(id);

            var response = mapper.Map<ClientDto>(clientDomain);

            return RepositoryResult<ClientDto>.Ok(response);
        }

        
    }
}






