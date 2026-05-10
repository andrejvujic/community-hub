using CommunityHub.Application.Domain.Issues;

namespace CommunityHub.Application.Domain.Issues.RepositoryInterfaces
{
    public interface IIssueRepository
    {
        public List<Issue> GetAllByBuilding(long buildingId);
        public List<Issue> GetAllByBuildingAndResident(long buildingId, long residentId);

        public int UpdateStatus(long issueId, IssueStatus newStatus);
    }
}
