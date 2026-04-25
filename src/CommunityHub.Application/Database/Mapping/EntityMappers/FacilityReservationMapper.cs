using System.Data;
using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.CityFacility;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class FacilityReservationMapper : EntityMapper<FacilityReservation>
    {
        private readonly CityFacilityMapper _facilityMapper = new CityFacilityMapper();
        private readonly NeighborhoodMapper _neighborhoodMapper = new NeighborhoodMapper();

        public override FacilityReservation? MapToEntity(IDataReader reader)
        {
            int idOrdinal = reader.GetOrdinal("facility_reservation_id");
            if (reader.IsDBNull(idOrdinal)) return null;
            long id = reader.GetInt64(idOrdinal);

            CityFacility? facility = _facilityMapper.MapToEntity(reader);
            Neighborhood? neighborhood = _neighborhoodMapper.MapToEntity(reader);
            if (facility is null || neighborhood is null) return null;

            DateTime startDatetime = reader.GetDateTime(reader.GetOrdinal("start_datetime"));
            DateTime endDatetime = reader.GetDateTime(reader.GetOrdinal("end_datetime"));

            return new FacilityReservation(facility, neighborhood, startDatetime, endDatetime, id);
        }

        public override Dictionary<string, object?> MapToQueryParameters(FacilityReservation entity)
        {
            throw new NotImplementedException();
        }
    }
}
