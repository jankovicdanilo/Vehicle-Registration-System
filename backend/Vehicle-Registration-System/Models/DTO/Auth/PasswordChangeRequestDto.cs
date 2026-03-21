namespace VehicleRegistrationSystem.Models.DTO.Auth
{
    public class PasswordChangeRequestDto
    {
        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
