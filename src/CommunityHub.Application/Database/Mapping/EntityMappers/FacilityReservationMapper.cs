using System.Data;
using CommunityHub.Application.Domain.CityFacility;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class FacilityReservationMapper : EntityMapper<FacilityReservation>
    {

        public override FacilityReservation? MapToEntity(IDataReader reader)
        {
            throw new NotImplementedException();
        }

        public override Dictionary<string, object?> MapToQueryParameters(FacilityReservation entity)
        {
            throw new NotImplementedException();
        }
    }
}
