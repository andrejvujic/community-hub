using CommunityHub.Application.Domain;
using System.Data;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class NeighborhoodMeetingMapper : EntityMapper<NeighborhoodMeeting>
    {
        private readonly NeighborhoodMapper _neighborhoodMapper = new NeighborhoodMapper();

        public override NeighborhoodMeeting MapToEntity(IDataReader reader)
        {
            throw new NotImplementedException();
        }

        public override Dictionary<string, object?> MapToQueryParameters(NeighborhoodMeeting entity)
        {
            throw new NotImplementedException();
        }
    }
}