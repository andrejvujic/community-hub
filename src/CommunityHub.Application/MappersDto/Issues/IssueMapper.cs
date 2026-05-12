using CommunityHub.Application.Services.Dtos.Issues;
using CommunityHub.Application.Domain.Issues;
using CommunityHub.Application.MappersDto.Users;
using CommunityHub.Application.MappersDto.Buildings;

namespace CommunityHub.Application.MappersDto.Issues
{
    public class IssueMapper : IMapper<Issue, IssueDto>
    {
        private readonly UserMapper _userMapper = new();
        private readonly BuildingMapper _buildingMapper = new();
        public IssueDto MapToDto(Issue issue)
        {
            if (issue == null) throw new ArgumentNullException(nameof(issue));

            return new IssueDto(
                issue.Id,
                issue.Description,
                issue.ReportedAt,
                issue.Priority,
                issue.Status,
                issue.ResidentId,
                issue.Resident != null ? _userMapper.MapToDto(issue.Resident) : null,
                issue.BuildingId,
                issue.Building != null ? _buildingMapper.MapToDto(issue.Building) : null
            );
        }
        public Issue MapToDomain(IssueDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new Issue(
                dto.Description,
                dto.ReportedAt,
                dto.Priority,
                dto.Status,
                dto.ResidentId,
                dto.BuildingId,
                dto.Resident != null ? _userMapper.MapToDomain(dto.Resident) : null,
                dto.Building != null ? _buildingMapper.MapToDomain(dto.Building) : null,
                dto.Id
            );
        }
    }
}