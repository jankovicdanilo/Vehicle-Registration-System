using AutoMapper;
using Azure.Core;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;
using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Models.DTO.Client;
using VehicleRegistrationSystem.Repositories.Interface;
using VehicleRegistrationSystem.Services.Implementation;

namespace VehicleRegistrationSystem.Tests.Services
{
    public class ClientServiceTests
    {
        private readonly Mock<IClientRepository> clientRepositoryMock;
        private readonly Mock<IRegistrationVehicleRepository> registrationRepositoryMock;
        private readonly Mock<ILogger<ClientService>> loggerMock;
        private readonly Mock<IMapper> mapperMock;

        private readonly ClientService clientService;

        public ClientServiceTests()
        {
            clientRepositoryMock = new Mock<IClientRepository>();
            registrationRepositoryMock = new Mock<IRegistrationVehicleRepository>();
            loggerMock = new Mock<ILogger<ClientService>>();
            mapperMock = new Mock<IMapper>();

            clientService = new ClientService(
                mapper: mapperMock.Object,
                clientRepositoryMock.Object,
                loggerMock.Object,
                registrationRepositoryMock.Object
            );
        }

        [Fact]
        public async Task ValidateClientCreateUpdateRequestAsyn_ShouldFail_WhenNationalIdExists()
        {
            var request = new CreateClientRequestDto
            {
                NationalId = "123456789",
                IdCardNumber = "ABC123",
                Email = "test@test.com"
            };

            clientRepositoryMock
                .SetupSequence(x => x.ExistsAsync(It.IsAny<Expression<Func<Client,bool>>>()))
                .ReturnsAsync(true);

            var result = await clientService.ValidateClientCreateRequestAsync(request);

            result.Success.Should().BeFalse();
            result.ErrorCode.Should().Be("NATIONAL_ID_EXISTS");
        }

        [Fact]
        public async Task ValidateClientCreateUpdateRequestAsyn_ShouldFail_WhenIdCardNumberExists()
        {
            var request = new CreateClientRequestDto
            {
                NationalId = "123456789",
                IdCardNumber = "ABC123",
                Email = "test@test.com"
            };

            clientRepositoryMock
                .SetupSequence(x => x.ExistsAsync(It.IsAny<Expression<Func<Client, bool>>>()))
                .ReturnsAsync(false)
                .ReturnsAsync (true);

            var result = await clientService.ValidateClientCreateRequestAsync(request);

            result.Success.Should().BeFalse();
            result.ErrorCode.Should().Be("ID_CARD_EXISTS");
        }

        [Fact]
        public async Task ValidateClientCreateUpdateRequestAsyn_ShouldFail_WhenEmailExists()
        {
            var request = new CreateClientRequestDto
            {
                NationalId = "123456789",
                IdCardNumber = "ABC123",
                Email = "test@test.com"
            };

            clientRepositoryMock
                .SetupSequence(x=>x.ExistsAsync(It.IsAny<Expression<Func<Client, bool>>>()))
                .ReturnsAsync(false)
                .ReturnsAsync(false)
                .ReturnsAsync (true);

            var result = await clientService.ValidateClientCreateRequestAsync(request);

            result.Success.Should().BeFalse();
            result.ErrorCode.Should().Be("EMAIL_EXISTS");
        }

        [Fact]
        public async Task ValidateClientCreateUpdateRequestAsync_ShouldPass_WhenAllFieldsAreUnique()
        {
            var request = new CreateClientRequestDto
            {
                NationalId = "123456789",
                IdCardNumber = "ABC123",
                Email = "test@test.com"
            };

            clientRepositoryMock
                .SetupSequence(x => x.ExistsAsync(It.IsAny<Expression<Func<Client, bool>>>()))
                .ReturnsAsync(false)
                .ReturnsAsync(false)
                .ReturnsAsync(false);

            var result = await clientService.ValidateClientCreateRequestAsync(request);

            result.Success.Should().BeTrue();
        }

        [Fact]
        public async Task ValidateClientId_ShouldFail_WhenClientDoesNotExist()
        {
            Guid id = Guid.NewGuid();

            clientRepositoryMock
                .Setup(x => x.ExistsAsync(It.IsAny<Expression<Func<Client, bool>>>()))
                .ReturnsAsync(false);

            var result = await clientService.ValidateClientId(id);

            result.Success.Should().BeFalse();
            result.ErrorCode.Should().Be("CLIENT_NOT_FOUND");
        }

        [Fact]
        public async Task ValidateClientDeleteRequestAsync_ShouldFail_WhenClientDoesNotExist()
        {
            Guid id = Guid.NewGuid();

            clientRepositoryMock
                .Setup(x => x.ExistsAsync(It.IsAny<Expression<Func<Client, bool>>>()))
                .ReturnsAsync(true);

            registrationRepositoryMock
                .Setup(x=>x.ExistsAsync(It.IsAny<Expression<Func<Registration,bool>>>()))
                .ReturnsAsync(true);

            var result = await clientService.ValidateClientDeleteRequestAsync(id);

            result.Success.Should().BeFalse();
            result.ErrorCode.Should().Be("CLIENT_REGISTRATION_EXISTS");
        }

        [Fact]
        public async Task DeleteClientAsync_ShouldReturnOk_WhenRequestIsValid()
        {
            Guid id = Guid.NewGuid();

            var clientDomain = new Client { Id = id };
            var clientDto = new ClientDto { Id = id };

            clientRepositoryMock
                .Setup(x=>x.ExistsAsync(It.IsAny<Expression<Func<Client,bool>>>()))
                .ReturnsAsync(true);

            registrationRepositoryMock
                .Setup(x => x.ExistsAsync(It.IsAny<Expression<Func<Registration, bool>>>()))
                .ReturnsAsync(false);

            clientRepositoryMock
                .Setup(x => x.DeleteAsync(id))
                .ReturnsAsync(clientDomain);

            mapperMock
                .Setup(x => x.Map<ClientDto>(It.IsAny<Client>()))
                .Returns(clientDto);

            var result = await clientService.DeleteClientAsync(id);

            result?.Success.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateClientAsync_ShouldReturnOk_WhenRequestIsValid()
        {
            var request = new UpdateClientRequestDto
            {
                NationalId = "123456789",
                IdCardNumber = "ABC123",
                Email = "test@test.com"
            };

            var clientDomain = new Client
            {
                Id = request.Id,
                IdCardNumber = request.IdCardNumber,
                NationalId = request.NationalId,
                Email = request.Email
            };

            var clientDto = new ClientDto
            {
                Id = request.Id
            };

            clientRepositoryMock
                .SetupSequence(x => x.ExistsAsync(It.IsAny<Expression<Func<Client, bool>>>()))
                .ReturnsAsync(true)
                .ReturnsAsync(false)
                .ReturnsAsync(false)
                .ReturnsAsync(false);

            mapperMock
                .Setup(x => x.Map<Client>(It.IsAny<UpdateClientRequestDto>()))
                .Returns(clientDomain);

            clientRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<Client>()))
                .ReturnsAsync(clientDomain);

            mapperMock
                .Setup(x => x.Map<ClientDto>(It.IsAny<Client>()))
                .Returns(clientDto);

            var result = await clientService.UpdateClientAsync(request);

            result.Success.Should().BeTrue();
            result.Data.Id.Should().Be(clientDto.Id);
        }

        [Fact]
        public async Task CreateClientAsync_ShouldReturnOk_WhenRequestIsValid()
        {
            var request = new CreateClientRequestDto
            {
                NationalId = "123456789",
                IdCardNumber = "ABC123",
                Email = "test@test.com"
            };

            var clientDomain = new Client
            {
                Id = Guid.NewGuid(),
                NationalId = request.NationalId,
                IdCardNumber = request.IdCardNumber,
                Email = request.Email,
            };

            var clientDto = new ClientDto
            {
                Id = clientDomain.Id
            };

            clientRepositoryMock
                .SetupSequence(x => x.ExistsAsync(It.IsAny<Expression<Func<Client, bool>>>()))
                .ReturnsAsync(false)
                .ReturnsAsync(false)
                .ReturnsAsync(false);

            clientRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Client>()))
                .ReturnsAsync(clientDomain);

            mapperMock
                .Setup(x => x.Map<Client>(It.IsAny<CreateClientRequestDto>()))
                .Returns(clientDomain);

            mapperMock
                .Setup(x => x.Map<ClientDto>(It.IsAny<Client>()))
                .Returns(clientDto);

            var result = await clientService.CreateClientAsync(request);

            result.Success.Should().BeTrue();
            result?.Data?.Id.Should().Be(clientDomain.Id);
        }

        [Fact]
        public async Task GetClientsAsync_ShouldReturnOk_WhenClientsExist()
        {
            var clients = new List<ClientListItemDto>
            {
                new ClientListItemDto {Id = Guid.NewGuid()},
                new ClientListItemDto {Id = Guid.NewGuid()}
            };

            clientRepositoryMock
                .Setup(x => x.GetAllAsync(
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<int>()))
                .ReturnsAsync((clients,2));

            var result = await clientService.GetClientsAsync();

            result.Success.Should().BeTrue();
            result.Data.Items.Should().HaveCount(2);
            result.Data.TotalCount.Should().Be(2);
        }

        [Fact]
        public async Task ValidateClientId_ShouldPass_WhenClientExists()
        {
            Guid id = Guid.NewGuid();

            clientRepositoryMock
                .Setup(x => x.ExistsAsync(It.IsAny<Expression<Func<Client,bool>>>()))
                .ReturnsAsync(true);

            var result = await clientService.ValidateClientId(id);

            result.Success.Should().BeTrue();
        }

        [Fact]
        public async Task GetClientByIdAsync_ShouldFail_WhenClientNotFound()
        {
            Guid id = Guid.NewGuid();

            clientRepositoryMock
                .Setup(x => x.ExistsAsync(It.IsAny<Expression<Func<Client,bool>>>()))
                .ReturnsAsync(false);

            var result = await clientService.GetClientByIdAsync(id);

            result.Success.Should().BeFalse();
            result.ErrorCode.Should().Be("CLIENT_NOT_FOUND");
        }

        [Fact]
        public async Task GetClientByIdAsync_ShouldReturnOk_WhenClientExists()
        {
            Guid id = Guid.NewGuid();
            var clientDomain = new Client { Id = id };
            var clientDto = new ClientDto { Id = id };

            clientRepositoryMock
                .Setup(x => x.ExistsAsync(It.IsAny<Expression<Func<Client,bool>>>()))
                .ReturnsAsync(true);

            clientRepositoryMock
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(clientDomain);

            mapperMock
                .Setup(x => x.Map<ClientDto>(It.IsAny<Client>()))
                .Returns(clientDto);

            var result = await clientService.GetClientByIdAsync(id);

            result.Success.Should().BeTrue();
            result.Data.Id.Should().Be(id);
        }
    }
}
