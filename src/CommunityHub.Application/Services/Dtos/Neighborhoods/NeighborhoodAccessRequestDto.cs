using CommunityHub.Application.Domain.Neighborhoods;
using CommunityHub.Application.Services.Dtos.Users;

namespace CommunityHub.Application.Services.Dtos.Neighborhoods
{
    public class NeighborhoodAccessRequestDto
    {
        public CitizenDto Citizen { get; set; }
        public NeighborhoodDto Neighborhood { get; set; }
        public NeighborhoodAccessRequestStatus Status { get; set; }
        public DateTime CreationDate { get; set; }
        public string? Comment { get; set; }

        public NeighborhoodAccessRequestDto(
            CitizenDto citizen,
            NeighborhoodDto neighborhood,
            NeighborhoodAccessRequestStatus status,
            DateTime creationDate,
            string? comment = null
        )
        {
            Citizen = citizen;
            Neighborhood = neighborhood;
            Status = status;
            CreationDate = creationDate;
            Comment = comment;
        }
    }
}
