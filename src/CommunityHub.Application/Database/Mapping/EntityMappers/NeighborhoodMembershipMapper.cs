using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.Neighborhoods;
using System.Data;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class NeighborhoodMembershipMapper : EntityMapper<NeighborhoodMembership>
    {
        private readonly NeighborhoodMapper _neighborhoodMapper = new NeighborhoodMapper();

        public override NeighborhoodMembership MapToEntity(IDataReader reader)
        {
            Neighborhood neighborhood = _neighborhoodMapper.MapToEntity(reader);

            long id = reader.GetInt64(reader.GetOrdinal("membership_id"));
            DateTime acceptanceDate = reader.GetDateTime(reader.GetOrdinal("acceptance_date"));

            return new NeighborhoodMembership(id, acceptanceDate, neighborhood);
        }

        public override Dictionary<string, object?> MapToQueryParameters(NeighborhoodMembership entity)
        {
            return new Dictionary<string, object?>
            {
                { "acceptance_date", entity.AcceptanceDate },
                { "neighborhood_id", entity.Neighborhood.Id }
            };
        }
    }
}