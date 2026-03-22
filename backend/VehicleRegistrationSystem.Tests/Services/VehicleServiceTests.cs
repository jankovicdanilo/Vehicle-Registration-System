using Moq;
using VehicleRegistrationSystem.Repositories.Interface;
using VehicleRegistrationSystem.Models.DTO.Vehicle;
using VehicleRegistrationSystem.Services.Implementation;
using AutoMapper;
using FluentAssertions;

namespace VehicleRegistrationSystem.Tests.Services
{
    public class VehicleServiceTests
    {
        private readonly Mock<IMapper> mapperMock;
        private readonly Mock<IVehicleRepository> vehicleRepositoryMock;
        private readonly Mock<IVehicleTypeRepository> vehicleTypeRepositoryMock;
        private readonly Mock<IVehicleBrandRepository> vehicleBrandRepositoryMock;
        private readonly Mock<IVehicleModelRepository> vehicleModelRepositoryMock;
        private readonly Mock<IRegistrationVehicleRepository> registrationVehicleRepositoryMock;

        private readonly VehicleService vehicleService;

        public VehicleServiceTests()
        {
            mapperMock = new Mock<IMapper>();
            vehicleRepositoryMock = new Mock<IVehicleRepository>();
            vehicleTypeRepositoryMock = new Mock<IVehicleTypeRepository>();
            vehicleBrandRepositoryMock = new Mock<IVehicleBrandRepository>();
            vehicleModelRepositoryMock = new Mock<IVehicleModelRepository>();
            registrationVehicleRepositoryMock = new Mock<IRegistrationVehicleRepository>();
            vehicleRepositoryMock = new Mock<IVehicleRepository>();

            vehicleService = new VehicleService(
                mapperMock.Object,
                vehicleRepositoryMock.Object,
                vehicleTypeRepositoryMock.Object,
                vehicleBrandRepositoryMock.Object,
                vehicleModelRepositoryMock.Object,
                registrationVehicleRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnVehicles_WhenVehiclesExist()
        {
            var vehicles = new List<VehicleListItemDto>
            {
                new VehicleListItemDto { Id = Guid.NewGuid() }
            };

            vehicleRepositoryMock
                .Setup(x => x.GetAllAsync(
                    It.IsAny<string?>(),
                    It.IsAny<int>(),
                    It.IsAny<int>()))
                .Returns(Task.FromResult((vehicles, 1)));

            var result = await vehicleService.GetAllAsync(null, 10, 1);

            result.Success.Should().BeTrue();
            result.Data.Items.Should().HaveCount(1);
        }
    }
}
