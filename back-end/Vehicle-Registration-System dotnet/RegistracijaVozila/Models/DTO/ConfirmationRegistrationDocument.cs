using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace VehicleRegistrationSystem.Models.DTO
{
    public class RegistrationConfirmationDocument : IDocument
    {
        private readonly RegistrationVehicleDto model;

        public RegistrationConfirmationDocument(RegistrationVehicleDto model)
        {
            this.model = model;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.Size(PageSizes.A4);

                page.Header().PaddingBottom(30).Text("Vehicle Registration Confirmation")
                .FontSize(20).Bold().AlignCenter();

                page.Content().Column(column =>
                {
                    column.Spacing(10);

                    column.Item().Text($"Owner First Name: {model.Client.FirstName}");
                    column.Item().Text($"Owner Last Name: {model.Client.LastName}");
                    column.Item().Text($"Email: {model.Client.Email}");
                    column.Item().Text($"ID Card Number: {model.Client.IdCardNumber}");
                    column.Item().Text($"National ID (JMBG): {model.Client.NationalId}");
                    column.Item().Text($"Address: {model.Client.Address}");
                    column.Item().Text($"Phone Number: {model.Client.PhoneNumber}");
                    column.Item().Text($"Vehicle Type: {model.Vehicle.VehicleTypeName}");
                    column.Item().Text($"Vehicle Brand: {model.Vehicle.VehicleBrandName}");
                    column.Item().Text($"Vehicle Model: {model.Vehicle.VehicleModelName}");
                    column.Item().Text($"License Plate: {model.LicensePlate}");
                    column.Item().Text($"Registration Date: {model.RegistrationDate:dd.MM.yyyy}");
                    column.Item().Text($"Registration Expiration Date: {model.ExpirationDate:dd.MM.yyyy}");
                    column.Item().Text($"Chassis Number: {model.Vehicle.ChassisNumber}");
                    column.Item().Text($"Engine Capacity: {model.Vehicle.EngineCapacity} cm3");
                    column.Item().Text($"Engine Power: {model.Vehicle.EnginePowerKw} kw");
                    column.Item().Text($"Production Year: {model.Vehicle.ProductionYear}");
                    column.Item().Text($"First Registration Date: {model.Vehicle.FirstRegistrationDate.Year}");
                    column.Item().Text($"Vehicle Weight: {model.Vehicle.Weight} kg");
                    column.Item().Text($"Fuel Type: {model.Vehicle.FuelType}");
                    column.Item().Text($"Insurance: {model.Insurance.Name}");
                    column.Item().Text($"Total Registration Price: {model.RegistrationPrice} dinars");
                });

                page.Footer().AlignCenter().Text(txt =>
                {
                    txt.Span("Ministry of Interior of the Republic of Serbia @ " + DateTime.Now.Year);
                });
            });
        }
    }
}