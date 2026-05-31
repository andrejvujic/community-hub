using System.Data;
using CommunityHub.Application.Domain.Neighborhoods.CityFacilities;
using CommunityHub.Application.Domain.Users;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class FacilityVoteMapper : EntityMapper<FacilityVote>
    {
        private readonly CityFacilityMapper _facilityMapper = new CityFacilityMapper();
        private readonly CitizenMapper _citizenMapper = new CitizenMapper();

        public override FacilityVote? MapToEntity(IDataReader reader)
        {
            int idOrdinal = reader.GetOrdinal("facility_vote_id");
            if (reader.IsDBNull(idOrdinal)) return null;
            long id = reader.GetInt64(idOrdinal);

            CityFacility? facility = _facilityMapper.MapToEntity(reader);
            Citizen? citizen = _citizenMapper.MapToEntity(reader);
            if (facility is null || citizen is null) return null;

            return new FacilityVote(facility, citizen, id);
        }

        public override Dictionary<string, object?> MapToQueryParameters(FacilityVote entity)
        {
            return new Dictionary<string, object?>
            {
                { "facility_id", entity.Facility.Id },
                { "citizen_id", entity.Citizen.Id }
            };
        }
    }
}
