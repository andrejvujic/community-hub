using CommunityHub.Application.Domain.Buildings.Issues;
using System.Data;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class IssueMapper: EntityMapper<Issue>
    {
        private readonly UserMapper _userMapper = new();
        private readonly BuildingMapper _buildingMapper = new();

        public override Issue MapToEntity(IDataReader reader)
        {
            var id = reader.GetInt64(reader.GetOrdinal("reported_issue_id"));

            var description = reader.GetString(reader.GetOrdinal("description"));

            var reportedAt = reader.GetDateTime(reader.GetOrdinal("reported_date"));

            var priority = (IssuePriority)reader.GetValue(reader.GetOrdinal("reported_issue_priority"));

            var status = (IssueStatus)reader.GetValue(reader.GetOrdinal("reported_issue_status"));

            var residentId = reader.GetInt64(reader.GetOrdinal("resident_id"));
            var resident = _userMapper.MapToEntity(reader.WithPrefix("resident"));

            var buildingId = reader.GetInt64(reader.GetOrdinal("building_id"));
            var building = _buildingMapper.MapToEntity(reader.WithPrefix("building"));

            return new Issue(description, reportedAt, priority, status, residentId, buildingId, resident, building, id);
        }

        public override Dictionary<string, object?> MapToQueryParameters(Issue entity)
        {
            return new()
            {
                { "description", entity.Description },
                { "reported_date", entity.ReportedAt },
                { "priority", entity.Priority },
                { "status", entity.Status },
                { "resident_id", entity.ResidentId },
                { "building_id", entity.BuildingId }
            };
        }
    }
}
