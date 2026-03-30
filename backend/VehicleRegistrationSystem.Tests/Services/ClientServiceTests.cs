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

        private readonly ClientService clientService;

        public ClientServiceTests()
        {
            clientRepositoryMock = new Mock<IClientRepository>();
            registrationRepositoryMock = new Mock<IRegistrationVehicleRepository>();
            loggerMock = new Mock<ILogger<ClientService>>();

            clientService = new ClientService(
                mapper: null,
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
    }
}
