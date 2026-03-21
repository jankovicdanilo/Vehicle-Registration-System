namespace VehicleRegistrationSystem.Models.DTO.Auth
{
    public class UserDto
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public IEnumerable<string> Roles { get; set; }

        public string MainRole { get; set; }
    }
}
