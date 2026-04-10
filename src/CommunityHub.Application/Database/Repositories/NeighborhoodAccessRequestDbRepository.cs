using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain;

namespace CommunityHub.Application.Database.Repositories
{
    public class NeighborhoodAccessRequestDbRepository
    {
        private readonly NeighborhoodAccessRequestMapper _neighborhoodAccessRequestMapper = new NeighborhoodAccessRequestMapper();

        public void Update(NeighborhoodAccessRequest request)
        {
            var query = PostgresQuery.Create(
                @"UPDATE neighborhood_access_requests 
                    SET status = @status, 
                    comment = @comment 
                    WHERE citizen_id = @citizen_id AND neighborhood_id = @neighborhood_id"
            )
            .ExtendParameters(_neighborhoodAccessRequestMapper.MapToQueryParameters(request));

            query.GetRowsAffectedCount();
        }
    }
}
