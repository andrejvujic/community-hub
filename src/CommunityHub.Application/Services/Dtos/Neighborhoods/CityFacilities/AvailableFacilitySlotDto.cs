namespace CommunityHub.Application.Services.Dtos.Neighborhoods.CityFacilities
{
    public class AvailableFacilitySlotDto
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool IsOutsideRange { get; set; }
        public int DurationHours { get; set; }

        public string DisplayText => $"{Start:dd.MM.yyyy.} u {Start:HH:mm} ({DurationHours}h)";

        public AvailableFacilitySlotDto(DateTime start, DateTime end, bool isOutsideRange)
        {
            Start = start;
            End = end;
            IsOutsideRange = isOutsideRange;
            DurationHours = (int)(end - start).TotalHours;
        }
    }
}