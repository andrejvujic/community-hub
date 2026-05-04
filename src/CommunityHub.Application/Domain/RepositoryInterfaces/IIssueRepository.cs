using CommunityHub.Application.Domain.Buildings.Issues;

namespace CommunityHub.Application.Domain.RepositoryInterfaces
{
    public interface IIssueRepository
    {
        public List<Issue> GetAllByBuilding(long buildingId);
        public List<Issue> GetAllByBuildingAndResident(long buildingId, long residentId);

        public int UpdateStatus(long issueId, IssueStatus newStatus);
    }
}
