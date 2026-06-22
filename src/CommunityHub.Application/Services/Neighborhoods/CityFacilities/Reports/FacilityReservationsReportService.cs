using CommunityHub.Application.Domain.RepositoryInterfaces.Neighborhoods.CityFacilities;
using CommunityHub.Application.MappersDto.Neighborhoods.CityFacilities;
using QuestPDF.Fluent;

namespace CommunityHub.Application.Services.Neighborhoods.CityFacilities.Reports
{
    public class FacilityReservationsReportService
    {
        private readonly IFacilityReservationRepository _reservationRepository
            = Injector.CreateInstance<IFacilityReservationRepository>();
        private readonly FacilityReservationMapper _reservationMapper = new();

        public string GenerateToDownloads(
            string neighborhoodName,
            long neighborhoodId,
            DateTime from,
            DateTime to)
        {
            var reservations = _reservationRepository
                .GetByNeighborhoodId(neighborhoodId)
                .Where(r => r.StartDatetime >= from && r.StartDatetime <= to)
                .Select(_reservationMapper.MapToDto)
                .ToList();

            var document = new FacilityReservationsReportPdfDocument(
                neighborhoodName,
                from,
                to,
                reservations);

            var downloadsPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "Downloads");

            var fileName = $"izvestaj-rezervacije-gradskih-objekata-{DateTime.Now:yyyyMMdd-HHmmss}.pdf";
            var filePath = Path.Combine(downloadsPath, fileName);

            document.GeneratePdf(filePath);

            return filePath;
        }

        public byte[] GenerateBytes(
            string neighborhoodName,
            long neighborhoodId,
            DateTime from,
            DateTime to,
            FacilityReservationsReportLabels? labels = null)
        {
            var reservations = _reservationRepository
                .GetByNeighborhoodId(neighborhoodId)
                .Where(r => r.StartDatetime >= from && r.StartDatetime <= to)
                .Select(_reservationMapper.MapToDto)
                .ToList();

            var document = new FacilityReservationsReportPdfDocument(
                neighborhoodName, from, to, reservations, labels);

            return document.GeneratePdf();
        }

        public string SaveToPath(byte[] pdfBytes, string filePath)
        {
            File.WriteAllBytes(filePath, pdfBytes);
            return filePath;
        }
    }
}