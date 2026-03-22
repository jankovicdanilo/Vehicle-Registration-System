namespace VehicleRegistrationSystem.Models.DTO.Registration
{
    public class RegistrationVehicleListItemDto
    {
        public Guid Id { get; set; }

        public DateTime RegistrationDate { get; set; }

        public DateTime ExpirationDate { get; set; }

        public decimal RegistrationPrice { get; set; }

        public string LicensePlate { get; set; } = string.Empty;

        public bool IsTemporary { get; set; }

        public string Vehicle { get; set; } = string.Empty ;

        public string Owner { get; set; } = string.Empty;

        public string Insurance { get; set; } = string.Empty;
    }
}
