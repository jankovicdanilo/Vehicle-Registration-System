using Microsoft.AspNetCore.Mvc;
using VehicleRegistrationSystem.Models.DTO;
using VehicleRegistrationSystem.Models.DTO.Common;
using VehicleRegistrationSystem.Models.DTO.InsurancePricing;
using VehicleRegistrationSystem.Services.Interface;

namespace VehicleRegistrationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsurancePricingController : ControllerBase
    {
        private readonly IInsurancePricingService insurancePricingService;

        public InsurancePricingController(IInsurancePricingService insurancePricingService)
        {
            this.insurancePricingService = insurancePricingService;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var result = await insurancePricingService.GetAllAsync();

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

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var result = await insurancePricingService.GetByIdAsync(id);

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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateInsurancePriceRequestDto request)
        {
            var result = await insurancePricingService.CreateAsync(request);

            if (!result.Success)
            {
                return BadRequest(new ApiError
                {
                    ErrorCode = result.ErrorCode,
                    Message = result.Message
                });
            }

            return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result);
        }
    }
}
