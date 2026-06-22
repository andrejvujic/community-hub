namespace CommunityHub.Application.Services.Neighborhoods.CityFacilities.Reports
{
    public class FacilityReservationsReportFormatter
    {
        private readonly FacilityReservationsReportLabels _labels;

        public FacilityReservationsReportFormatter(FacilityReservationsReportLabels labels)
        {
            _labels = labels;
        }

        public string FormatDate(DateTime date)
            => date.ToString(_labels.DateFormat, _labels.Culture);

        public string FormatTime(DateTime dateTime)
            => dateTime.ToString(_labels.TimeFormat, _labels.Culture);

        public string FormatDateRange(DateTime from, DateTime to)
            => $"{FormatDate(from)} - {FormatDate(to)}";

        public string FormatDateTime(DateTime dateTime)
            => $"{FormatDate(dateTime)} {_labels.AtWord} {FormatTime(dateTime)}";

        public string FormatDuration(DateTime start, DateTime end)
        {
            var duration = (int)(end - start).TotalHours;
            return $"{duration}h";
        }

        public string FormatGeneratedAt()
            => DateTime.Now.ToString($"{_labels.DateFormat} {_labels.TimeFormat}", _labels.Culture);
    }
}