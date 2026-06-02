using CommunityHub.Application.Domain.Issues;
using CommunityHub.Application.Domain.RepositoryInterfaces.Issues;
using CommunityHub.Application.MappersDto.Issues;
using CommunityHub.Application.Services.Dtos.Issues;
using CommunityHub.Application.Services.Buildings;

namespace CommunityHub.Application.Services.Issues
{
    public class IssueService : BaseService<Issue, IssueDto>
    {
        private readonly IIssueRepository _issueRepository = Injector.CreateInstance<IIssueRepository>();
        private readonly BuildingService _buildingService = new();

        public IssueService() : base(new IssueMapper()) { }

        public List<IssueDto> GetAllByBuilding(long buildingId)
            => MapToDto(_issueRepository.GetAllByBuilding(buildingId));

        public List<IssueDto> GetAllByBuildingAndResident(long buildingId, long residentId)
            => MapToDto(_issueRepository.GetAllByBuildingAndResident(buildingId, residentId));

        public IssueDto Create(IssueDto issue)
            => MapToDto(_issueRepository.Create(MapToEntity(issue)));

        public int MarkIssueAsResolved(IssueDto issueDto)
        {
            var issue = MapToEntity(issueDto);

            issue = issue.CopyWith(status: IssueStatus.PotentiallyResolved);

            return _issueRepository.Update(issue);
        }

        public IssueDto ConfirmResolution(IssueDto issueDto)
        {
            var issue = MapToEntity(issueDto);
            issue.ConfirmResolution();
            _issueRepository.Update(issue);

            return MapToDto(issue);
        }

        public int CountOpenIssuesForManagerBuildings(long managerId)
        {
            var managerBuildings = _buildingService.GetByManagerId(managerId);

            var issues = managerBuildings
               .SelectMany(building => GetAllByBuilding(building.Id))
               .Where(issue => issue.Status == IssueStatus.Open);

            return issues.Count();
        }

        public List<BuildingOpenIssuesChartDto> GetOpenIssuesChartDataForManager(long managerId)
        {
            var managerBuildings = _buildingService.GetByManagerId(managerId);

            var issues = managerBuildings
               .SelectMany(building => GetAllByBuilding(building.Id))
               .Where(issue => issue.Status == IssueStatus.Open);

            var result = managerBuildings
                .Select(building => new BuildingOpenIssuesChartDto
                {
                    BuildingInfo = building.DisplayAddress,
                    OpenIssuesCount = issues.Count(issue => issue.BuildingId == building.Id)
                })
                .OrderByDescending(x => x.OpenIssuesCount)
                .ToList();

            return result;
        }
    }
}
