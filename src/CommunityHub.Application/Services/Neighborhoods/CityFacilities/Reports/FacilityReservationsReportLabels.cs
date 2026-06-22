using System.Globalization;

namespace CommunityHub.Application.Services.Neighborhoods.CityFacilities.Reports
{
    public class FacilityReservationsReportLabels
    {
        public CultureInfo Culture { get; set; } = new("sr-Latn-RS");

        public string HeaderTitle { get; set; } = "Izveštaj o rezervacijama gradskih objekata";
        public string NeighborhoodLabel { get; set; } = "Kvart: ";
        public string PeriodLabel { get; set; } = "Period: ";
        public string TotalReservationsLabel { get; set; } = "Ukupno rezervacija: ";
        public string GeneratedAtLabel { get; set; } = "Generisano: ";
        public string NoReservationsMessage { get; set; } = "Nema rezervacija gradskih objekata za izabrani period.";
        public string VisitCountLabel { get; set; } = "Broj poseta: ";
        public string VisitDateLabel { get; set; } = "Datum posete";
        public string StartEndLabel { get; set; } = "Početak / Kraj";
        public string DurationLabel { get; set; } = "Trajanje";
        public string FooterBrand { get; set; } = "CommunityHub";
        public string PageLabel { get; set; } = "Strana ";
        public string PageOfSeparator { get; set; } = " / ";

        public string AtWord { get; set; } = "u";

        public string DateFormat { get; set; } = "dd.MM.yyyy.";
        public string TimeFormat { get; set; } = "HH:mm";
    }
}