namespace VehicleRegistrationSystem.Models.DTO
{
    public class PasswordChangeRequestDto
    {
        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
