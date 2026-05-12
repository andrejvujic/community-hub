using CommunityHub.Application.Domain.Issues;
using CommunityHub.Application.Services.Dtos.Buildings;
using CommunityHub.Application.Services.Dtos.Users;

namespace CommunityHub.Application.Services.Dtos.Issues
{
    public class IssueDto
    {
        public long Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime ReportedAt { get; set; }

        public IssuePriority Priority { get; set; }
        public IssueStatus Status { get; set; }

        public long ResidentId { get; set; }
        public UserDto? Resident { get; set; }

        public long BuildingId { get; set; }
        public BuildingDto? Building { get; set; }

        public IssueDto(
            long id,
            string description,
            DateTime reportedAt,
            IssuePriority priority,
            IssueStatus status,
            long residentId,
            UserDto? resident,
            long buildingId,
            BuildingDto? building
        )
        {
            Id = id;
            Description = description;
            ReportedAt = reportedAt;
            Priority = priority;
            Status = status;
            ResidentId = residentId;
            Resident = resident;
            BuildingId = buildingId;
            Building = building;
        }
    }
}