using VehicleRegistrationSystem.Models.Domain;

namespace VehicleRegistrationSystem.Models.Domain
{
    public class Client
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string NationalId { get; set; }

        public string IdCardNumber { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public DateTime DateOfBirth { get; set; }

        public ICollection<Registration> Registrations { get; set; } = new HashSet<Registration>();
    }
}
