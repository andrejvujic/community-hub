namespace CommunityHub.Application.Domain.Neighborhoods.CityFacility
{
    public class AvailableFacilitySlot
    {
        public DateTime Start { get; }
        public DateTime End { get; }
        public bool IsOutsideRange { get; }
        public int DurationHours { get; }

        public AvailableFacilitySlot(DateTime start, DateTime end, bool isOutsideRange)
        {
            Start = start;
            End = end;
            IsOutsideRange = isOutsideRange;
            DurationHours = (int)(end - start).TotalHours;
        }

        public string DisplayText =>
            $"{Start:dd.MM.yyyy.} u {Start:HH:mm} ({DurationHours}h)";
    }
}
