using CommunityHub.Application.Services.Dtos.Addresses;
using CommunityHub.Application.Services.Dtos.Neighborhoods;

namespace CommunityHub.Application.Services.Dtos.Neighborhoods.CityFacilities
{
    public class FacilityReservationDto
    {
        public long Id { get; set; }
        public CityFacilityDto Facility { get; set; }
        public NeighborhoodDto Neighborhood { get; set; }
        public DateTime StartDatetime { get; set; }
        public DateTime EndDatetime { get; set; }

        public FacilityReservationDto(
            long id,
            CityFacilityDto facility,
            NeighborhoodDto neighborhood,
            DateTime startDatetime,
            DateTime endDatetime
        )
        {
            Id = id;
            Facility = facility;
            Neighborhood = neighborhood;
            StartDatetime = startDatetime;
            EndDatetime = endDatetime;
        }
    }
}
