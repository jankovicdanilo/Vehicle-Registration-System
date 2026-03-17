using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using VehicleRegistrationSystem.Models.DTO;
using VehicleRegistrationSystem.Services.Interface;

namespace VehicleRegistrationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationVehicleController : ControllerBase
    {
        private readonly IRegistrationVehicleService registrationVehicleService;
        private readonly IEmailService emailService;

        public RegistrationVehicleController(IRegistrationVehicleService registrationVehicleService,
            IEmailService emailService)
        {
            this.registrationVehicleService = registrationVehicleService;
            this.emailService = emailService;
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRegistrationVehicleRequestDto request)
        {
            var result = await registrationVehicleService.CreateRegistrationAsync(request);

            if (!result.Success)
            {
                var parts = result.Message?.Split(":", 2);

                return BadRequest(new ApiError
                {
                    ErrorCode = parts?[0],
                    Message = parts?[1].Length > 1 ? parts?[1] : result.Message
                });
            }

            //var confirmationData = await registrationVehicleService.GenerateConfirmation(result.Data.Id);
            //var document = new RegistrationConfirmationDocument(confirmationData.Data);
            //var pdfBytes = document.GeneratePdf();
            //await emailService.SendConfirmationEmailAsync(confirmationData.Data.Client.Email, pdfBytes);

            return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result);
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] string? searchQuery, [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 1000)
        {
            var result = await 
                registrationVehicleService.GetAllAsync(searchQuery, pageNumber, pageSize);

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await registrationVehicleService.GetByIdAsync(id);

            if (!result.Success)
            {
                var parts = result.Message?.Split(":", 2);

                return BadRequest(new ApiError
                {
                    ErrorCode = parts?[0],
                    Message = parts?[1].Length > 1 ? parts?[1] : result.Message
                });
            }

            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await registrationVehicleService.DeleteRegistrationAsync(id);

            if (!result.Success)
            {
                var parts = result.Message?.Split(":", 2);

                return BadRequest(new ApiError
                {
                    ErrorCode = parts?[0],
                    Message = parts?[1].Length > 1 ? parts?[1] : result.Message
                });
            }
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateRegistrationVehicleRequestDto request)
        {
            var result = await registrationVehicleService.UpdateRegistrationAsync(request);

            if (!result.Success)
            {
                var parts = result.Message?.Split(":", 2);

                return BadRequest(new ApiError
                {
                    ErrorCode = parts?[0],
                    Message = parts?[1].Length > 1 ? parts?[1] : result.Message
                });
            }

            return Ok(result);
        }

        [HttpGet("confirmation/{id}")]
        public async Task<IActionResult> GenerateConfirmation(Guid id)
        {
            var result = await registrationVehicleService.GenerateConfirmation(id);

            var document = new RegistrationConfirmationDocument(result.Data);
            var pdfBytes = document.GeneratePdf();

            return File(pdfBytes, "application/pdf", "RegistrationConfirmation.pdf");
        }

        [HttpGet("confirmation-email/{id}")]
        public async Task<IActionResult> GenerateAndSendConfirmation(Guid id)
        {
            var result = await registrationVehicleService.GenerateConfirmation(id);

            var document = new RegistrationConfirmationDocument(result.Data);
            var pdfBytes = document.GeneratePdf();

            await emailService.SendConfirmationEmailAsync(result.Data.Client.Email, pdfBytes);

            return Ok("PDF confirmation has been sent via email");
        }
    }
}



