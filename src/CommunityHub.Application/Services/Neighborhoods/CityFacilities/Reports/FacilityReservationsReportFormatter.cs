using System.Globalization;

namespace CommunityHub.Application.Services.Neighborhoods.CityFacilities.Reports
{
    public class FacilityReservationsReportFormatter
    {
        private static readonly CultureInfo Culture = new("sr-Latn-RS");

        public string FormatDate(DateTime date)
            => date.ToString("dd.MM.yyyy.", Culture);

        public string FormatTime(DateTime dateTime)
            => dateTime.ToString("HH:mm", Culture);

        public string FormatDateRange(DateTime from, DateTime to)
            => $"{FormatDate(from)} - {FormatDate(to)}";

        public string FormatDateTime(DateTime dateTime)
            => $"{FormatDate(dateTime)} u {FormatTime(dateTime)}";

        public string FormatDuration(DateTime start, DateTime end)
        {
            var duration = (int)(end - start).TotalHours;
            return $"{duration}h";
        }

        public string FormatGeneratedAt()
            => DateTime.Now.ToString("dd.MM.yyyy. HH:mm", Culture) + "h";
    }
}