using CommunityHub.Application.Domain.Issues;

namespace CommunityHub.Application.Domain.RepositoryInterfaces.Issues
{
    public interface IIssueRepository
    {
        public Issue Create(Issue issue);
        public List<Issue> GetAllByBuilding(long buildingId);
        public List<Issue> GetAllByBuildingAndResident(long buildingId, long residentId);
        public int Update(Issue issue);
    }
}
