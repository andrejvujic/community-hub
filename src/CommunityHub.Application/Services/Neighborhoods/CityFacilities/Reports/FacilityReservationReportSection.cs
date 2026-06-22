using CommunityHub.Application.Services.Dtos.Neighborhoods.CityFacilities;

namespace CommunityHub.Application.Services.Neighborhoods.CityFacilities.Reports
{
    public class FacilityReservationReportSection
    {
        public string FacilityName { get; set; } = string.Empty;
        public List<FacilityReservationDto> Items { get; set; } = new();
    }
}
