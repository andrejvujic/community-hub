using System.Data;
using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.CityFacility;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class CityFacilityMapper : EntityMapper<CityFacility>
    {
        public override CityFacility? MapToEntity(IDataReader reader)
        {
            throw new NotImplementedException();
        }

        public override Dictionary<string, object?> MapToQueryParameters(CityFacility entity)
        {
            throw new NotImplementedException();
        }
    }
}
