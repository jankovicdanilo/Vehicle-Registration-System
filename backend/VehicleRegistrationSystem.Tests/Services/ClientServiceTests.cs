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
        public async Task ValidateClientCreateRequestAsync_ShouldFail_WhenNationalIdExists()
        {
            var request = new CreateClientRequestDto
            {
                NationalId = "123456789"
            };

            clientRepositoryMock
                .Setup(x => x.ExistsAsync(It.IsAny<Expression<Func<Client,bool>>>()))
                .ReturnsAsync(true);

            var result = await clientService.ValidateClientCreateRequestAsync(request);

            result.Success.Should().BeFalse();
        }

        public async Task ValidateClientCreateRequestAsync_ShouldPass_WhenNationalIdDoesNotExist()
        {
            var request = new CreateClientRequestDto
            {
                NationalId = "123456789"
            };

            clientRepositoryMock
                .Setup(x => x.ExistsAsync(It.IsAny<Expression<Func<Client, bool>>>()))
                .ReturnsAsync(false);

            var result = await clientService.ValidateClientCreateRequestAsync(request);

            result.Success!.Should().BeTrue();
        }
    }
}
