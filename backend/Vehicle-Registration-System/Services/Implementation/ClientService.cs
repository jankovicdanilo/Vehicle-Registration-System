using AutoMapper;
using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Repositories.Interface;
using VehicleRegistrationSystem.Results;
using VehicleRegistrationSystem.Services.Interface;
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

        public async Task<Result<bool>> ValidateClientCreateRequestAsync
            (CreateClientRequestDto request)
        {
            return await ValidateUniqueClientFields(
                request.NationalId,
                request.IdCardNumber,
                request.Email);
        }

        public async Task<Result<ClientDto>> CreateClientAsync
            (CreateClientRequestDto request)
        {
            logger.LogInformation("Starting client creation for email {Email}", request.Email);

            var validationResult = await ValidateClientCreateRequestAsync(request);

            if (!validationResult.Success)
            {
                logger.LogWarning("Client creation validation failed for email {Email}: {Message}",
                    request.Email, validationResult.Message);

                return Result<ClientDto>.Fail(validationResult.ErrorCode,validationResult.Message);
            }

            var clientDomain = mapper.Map<Client>(request);

            clientDomain = await clientRepository.AddAsync(clientDomain);

            var response = mapper.Map<ClientDto>(clientDomain);

            logger.LogInformation("Client successfully created with ID {ClientId}", response.Id);

            return Result<ClientDto>.Ok(response, "Client successfully created!");
        }

        public async Task<Result<bool>> ValidateClientDeleteRequestAsync(Guid id)
        {
            var exists = await clientRepository.ExistsAsync(x => x.Id == id);

            if(!exists)
            {
                return Result<bool>.Fail("CLIENT_NOT_FOUND",$"Client with the Id {id} was not found");
            }

            var isOwner = await registrationVehicleRepository.ExistsAsync(x => x.ClientId == id);

            if (isOwner)
            {
                return Result<bool>.Fail
                    ("CLIENT_REGISTRATION_EXISTS","Client can't be deleted because vehicles are " +
                    "still registered under this client.");
            }

            return Result<bool>.Ok(true);
        }

        public async Task<Result<ClientDto>> DeleteClientAsync(Guid id)
        {
            var validationResult = await ValidateClientDeleteRequestAsync(id);

            if (!validationResult.Success)
            {
                return Result<ClientDto>.Fail(validationResult.ErrorCode,validationResult.Message);
            }

            var existingClient = await clientRepository.DeleteAsync(id);

            var response = mapper.Map<ClientDto>(existingClient);

            return Result<ClientDto>.Ok(response, "Client successfully deleted!");
        }

        public async Task<Result<bool>> ValidateClientUpdateRequestAsync
            (UpdateClientRequestDto request)
        {
            var exists = await clientRepository.ExistsAsync(x => x.Id == request.Id);

            if (!exists)
            {
                return Result<bool>.Fail
                    ("CLIENT_NOT_FOUND",$"Client with the Id {request.Id} was not found");
            }

            return await ValidateUniqueClientFields(
                request.NationalId,
                request.IdCardNumber,
                request.Email);
        }

        public async Task<Result<ClientDto>> UpdateClientAsync
            (UpdateClientRequestDto request)
        {
            var validationResult = await ValidateClientUpdateRequestAsync(request);

            if (!validationResult.Success)
            {
                return Result<ClientDto>.Fail
                    (validationResult.ErrorCode,validationResult.Message);
            }

            var clientDomain = mapper.Map<Client>(request);

            clientDomain = await clientRepository.UpdateClientAsync(clientDomain);

            var response = mapper.Map<ClientDto>(clientDomain);

            return Result<ClientDto>.Ok(response, "Client successfully updated!");
        }

        public async Task<Result<PagedResult<ClientListItemDto>>> GetClientsAsync
            (string? searchQuery = null, int pageNumber = 1, int pageSize = 10)
        {
            var (clients, totalCount) = await clientRepository.
                GetAllAsync(searchQuery, pageNumber, pageSize);

            var response = new PagedResult<ClientListItemDto>
            {
                Items = clients,
                TotalCount = totalCount
            };


            return Result<PagedResult<ClientListItemDto>>.Ok(response, 
                "Clients loaded successfully");
        }

        public async Task<Result<bool>> ValidateClientId(Guid id)
        {
            if (!await clientRepository.ExistsAsync(x => x.Id == id))
            {
                return Result<bool>.Fail("CLIENT_NOT_FOUND", $"Client with the Id " +
                    $"{id} was not found");
            }

            return Result<bool>.Ok(true);
        }

        public async Task<Result<ClientDto>> GetClientByIdAsync(Guid id)
        {
            var validationResult = await ValidateClientId(id);

            if (!validationResult.Success)
            {
                return Result<ClientDto>.Fail(validationResult.ErrorCode,validationResult.Message);
            }

            var clientDomain = await clientRepository.GetByIdAsync(id);

            var response = mapper.Map<ClientDto>(clientDomain);

            return Result<ClientDto>.Ok(response);
        }

        private async Task<Result<bool>> ValidateUniqueClientFields(string nationalId, 
            string idCardNumber, string email, Guid? excludeId = null)
        {
            if (await clientRepository.ExistsAsync(x => x.NationalId == nationalId && 
                    (excludeId == null || x.Id != excludeId)))
            {
                return Result<bool>.Fail
                    ("NATIONAL_ID_EXISTS", "A client with this social " +
                    "security number already exists");
            }

            if (await clientRepository.ExistsAsync
                (x => x.IdCardNumber == idCardNumber && 
                    (excludeId == null || x.Id !=excludeId)))
            {
                return Result<bool>.Fail
                    ("ID_CARD_EXISTS", "A client with this ID card number already exists");
            }

            if (await clientRepository.ExistsAsync(x => x.Email == email && 
            (excludeId == null || x.Id != excludeId)))
            {
                return Result<bool>.Fail
                    ("EMAIL_EXISTS", "A client with this email adress already exists");
            }

            return Result<bool>.Ok(true);
        }
    }
}






