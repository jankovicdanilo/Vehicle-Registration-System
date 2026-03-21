namespace VehicleRegistrationSystem.Models.DTO.Common
{
    public class ConfirmationRegistrationDto
    {
        public string OwnerName { get; set; }

        public string VehicleBrand { get; set; }

        public string VehicleModel { get; set; }

        public string LicencePlate { get; set; }

        public DateTime RegistrationDate { get; set; }

        public DateTime ExpirationDate { get; set; }

        public decimal RegistrationPrice { get; set; }
    }
}
