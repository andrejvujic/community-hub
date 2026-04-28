
namespace CommunityHub.Application.Domain.ReportedIsssue
{
    public class ReportedIssue
    {
        public long Id { get; private set; }
        public string Description { get; private set; } = string.Empty;
        public DateTime ReportedAt { get; private set; }
        public ReportedIssuePriority Priority { get; private set; }
        public ReportedIssueStatus Status { get; private set; }
        public long ResidentId { get; private set; }
        public User? Resident { get; private set; }
        public long BuildingId { get; private set; }
        public Building? Building { get; private set; }

        public ReportedIssue(
            string description,
            DateTime reportedAt,
            ReportedIssuePriority priority,
            ReportedIssueStatus status,
            long residentId,
            long buildingId,
            User? resident = null,
            Building? building = null,
            long id = 0
        )
        {
            Description = description;
            ReportedAt = reportedAt;
            Priority = priority;
            Status = status;
            ResidentId = residentId;
            Resident = resident;
            BuildingId = buildingId;
            Building = building;
            Id = id;
        }
    }
}
