using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VehicleRegistrationSystem.Models.DTO;
using VehicleRegistrationSystem.Models.DTO.Client;
using VehicleRegistrationSystem.Models.DTO.Common;
using VehicleRegistrationSystem.Repositories.Interface;
using VehicleRegistrationSystem.Results;
using VehicleRegistrationSystem.Services.Interface;

namespace VehicleRegistrationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService clientService;

        public ClientController(IClientRepository clientRepository, IMapper mapper,
            IRegistrationVehicleRepository registrationVehicleRepository, IClientService clientService)
        {
            this.clientService = clientService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateClientRequestDto request)
        {
            var result = await clientService.CreateClientAsync(request);

            if (!result.Success)
            {
                return BadRequest(new ApiError
                {
                    ErrorCode = result.ErrorCode,
                    Message = result.Message
                });
            }

            return CreatedAtAction(nameof(GetClientById), new { id = result.Data.Id}, result);
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] string? searchQuery,[FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var response = await clientService.GetClientsAsync(searchQuery, pageNumber, pageSize);
            
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetClientById([FromRoute] Guid id)
        {
            var response = await clientService.GetClientByIdAsync(id);

            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await clientService.DeleteClientAsync(id);

            if (!result.Success)
            {
                return BadRequest(new ApiError
                {
                    ErrorCode = result.ErrorCode,
                    Message = result.Message
                });
            }

            return Ok(result);

        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateClientRequestDto updateRequest)
        {
            var result = await clientService.UpdateClientAsync(updateRequest);

            if (!result.Success)
            {
                return BadRequest(new ApiError
                {
                    ErrorCode = result.ErrorCode,
                    Message = result.Message
                });
            }

            return Ok(result);
        }

    }
}
