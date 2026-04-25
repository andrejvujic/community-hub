namespace CommunityHub.Application.Domain.CityFacility
{
    public class FacilityReservation
    {
        public long Id { get; private set; }
        public CityFacility Facility { get; private set; }
        public Neighborhood Neighborhood { get; private set; }
        public DateTime StartDatetime { get; private set; }
        public DateTime EndDatetime { get; private set; }

        public FacilityReservation(
            CityFacility facility,
            Neighborhood neighborhood,
            DateTime startDatetime,
            DateTime endDatetime,
            long id = 0)
        {
            if (startDatetime >= endDatetime)
                throw new ArgumentException("Početak rezervacije mora biti pre kraja.");

            Id = id;
            Facility = facility;
            Neighborhood = neighborhood;
            StartDatetime = startDatetime;
            EndDatetime = endDatetime;
        }

        public FacilityReservation CopyWith(
            long? id = null,
            CityFacility? facility = null,
            Neighborhood? neighborhood = null,
            DateTime? startDatetime = null,
            DateTime? endDatetime = null)
        {
            return new FacilityReservation(
                facility ?? Facility,
                neighborhood ?? Neighborhood,
                startDatetime ?? StartDatetime,
                endDatetime ?? EndDatetime,
                id ?? Id
            );
        }
    }
}
