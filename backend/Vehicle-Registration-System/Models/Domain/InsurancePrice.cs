namespace VehicleRegistrationSystem.Models.Domain
{
    public class InsurancePrice
    {
        public Guid Id { get; set; }

        public Guid InsuranceId { get; set; }

        public Insurance Insurance { get; set; }

        public int MinKw { get; set; }

        public int MaxKw { get; set; }

        public decimal PricePerKw { get; set; }
    }
}