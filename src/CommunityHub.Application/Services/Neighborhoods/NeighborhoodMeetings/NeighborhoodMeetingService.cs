using CommunityHub.Application.Domain.Neighborhoods.NeighborhoodMeetings.RepositoryInterfaces;
using CommunityHub.Application.MappersDto.Neighborhoods.NeighborhoodMeetings;

namespace CommunityHub.Application.Services.Neighborhoods.NeighborhoodMeetings
{
    public class NeighborhoodMeetingService
    {
        private readonly INeighborhoodMeetingRepository _neighborhoodMeetingRepository 
            = Injector.CreateInstance<INeighborhoodMeetingRepository>();
        private readonly NeighborhoodMeetingMapper _neighborhoodMeetingMapper = new();

    }
}
