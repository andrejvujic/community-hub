using CommunityHub.Application.MappersDto.Issues;
using CommunityHub.Application.Services.Dtos.Issues;
using CommunityHub.Application.Domain.Issues;
using CommunityHub.Application.Domain.RepositoryInterfaces.Issues;

namespace CommunityHub.Application.Services.Issues
{
    public class IssueService : BaseService<Issue, IssueDto>
    {
        private readonly IIssueRepository _issueRepository = Injector.CreateInstance<IIssueRepository>();
        private readonly IssueMapper _issueMapper = new();

        public IssueService() : base(new IssueMapper()) { }

        public List<IssueDto> GetAllByBuilding(long buildingId)
        {
            var issues = _issueRepository.GetAllByBuilding(buildingId);

            return issues.Select(_issueMapper.MapToDto).ToList();
        }

        public List<IssueDto> GetAllByBuildingAndResident(long buildingId, long residentId)
            => MapToDto(_issueRepository.GetAllByBuildingAndResident(buildingId, residentId));

        public int MarkIssueAsResolved(IssueDto issueDto)
        {
            var issue = _issueMapper.MapToDomain(issueDto);

            issue = issue.CopyWith(status: IssueStatus.PotentiallyResolved);

            return _issueRepository.Update(issue);
        }
    }
}
