using System.ComponentModel.DataAnnotations;

namespace VehicleRegistrationSystem.Models.DTO.Client
{
    public class CreateClientRequestDto
    {
        [Required(ErrorMessage ="Name of client is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Surname of client is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Social security number of client is required")]
        public string NationalId { get; set; }

        [Required(ErrorMessage = "Id carb number of client is required")]
        public string IdCardNumber { get; set; }

        [Required(ErrorMessage = "Email of client is required")]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}


