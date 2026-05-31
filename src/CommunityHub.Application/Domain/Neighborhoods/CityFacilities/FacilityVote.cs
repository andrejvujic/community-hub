using CommunityHub.Application.Domain.Users;

namespace CommunityHub.Application.Domain.Neighborhoods.CityFacilities
{
    public class FacilityVote
    {
        public long Id { get; private set; }
        public CityFacility Facility { get; private set; }
        public Citizen Citizen { get; private set; }

        public FacilityVote(
            CityFacility facility,
            Citizen citizen,
            long id = 0
        )
        {
            Id = id;
            Facility = facility;
            Citizen = citizen;
        }

        public FacilityVote CopyWith(
            long? id = null,
            CityFacility? facility = null,
            Citizen? citizen = null
        )
        {
            return new FacilityVote(
                facility ?? Facility,
                citizen ?? Citizen,
                id ?? Id
            );
        }
    }
}
