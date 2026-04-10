using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain;

namespace CommunityHub.Application.Database.Repositories
{

    public class NeighborhoodMembershipDbRepository
    {
        private readonly NeighborhoodMembershipMapper _neighborhoodMembershipMapper = new NeighborhoodMembershipMapper();

        public NeighborhoodMembership Create(NeighborhoodMembership membership)
        {
            var query = PostgresQuery.Create(
                @"INSERT INTO neighborhood_memberships (acceptance_date, neighborhood_id) 
                VALUES (@acceptance_date, @neighborhood_id) 
                RETURNING id"
            ).ExtendParameters(_neighborhoodMembershipMapper.MapToQueryParameters(membership));

            long generatedId = query.ExecuteScalar<long>(value => Convert.ToInt64(value));
            
            return membership.CopyWith(id: generatedId);
        }
    }
}
