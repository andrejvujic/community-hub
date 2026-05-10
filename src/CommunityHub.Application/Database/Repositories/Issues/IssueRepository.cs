using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain.Issues;
using CommunityHub.Application.Domain.Issues.RepositoryInterfaces;

namespace CommunityHub.Application.Database.Repositories.Issues
{
	public class IssueRepository : IIssueRepository
    {
		private readonly IssueMapper _issueMapper = new();

		public List<Issue> GetAllByBuilding(long buildingId)
		{
			var query = PostgresQuery.Create(
				@"SELECT
					reported_issue.id AS reported_issue_id,
					reported_issue.description,
					reported_issue.reported_date,
					reported_issue.priority AS reported_issue_priority,
					reported_issue.status AS reported_issue_status,
					reported_issue.building_id,

					reported_issue.resident_id,
					u.id AS resident_user_id,
					u.username AS resident_username,
					u.user_role AS resident_user_role,
					u.password AS resident_password,
					u.name AS resident_name,
					u.surname AS resident_surname
	
					FROM reported_issues reported_issue INNER JOIN users u on reported_issue.resident_id = u.id 
					WHERE reported_issue.building_id = @building_id
					ORDER BY reported_issue.reported_date ASC;"
				)
				.AddParameter("@building_id", buildingId);

			List<Issue> issues = new List<Issue>();

			query.ExecuteReader(reader => issues.Add(_issueMapper.MapToEntity(reader)));

			return issues;
		}

		public List<Issue> GetAllByBuildingAndResident(long buildingId, long residentId)
		{
            var query = PostgresQuery.Create(
                @"SELECT
					issue.id AS reported_issue_id,
					issue.description,
					issue.reported_date,
					issue.priority AS reported_issue_priority,
					issue.status AS reported_issue_status,
					issue.building_id,

					issue.resident_id,

					resident.id AS resident_user_id,
					resident.username AS resident_username,
					resident.user_role AS resident_user_role,
					resident.password AS resident_password,
					resident.name AS resident_name,
					resident.surname AS resident_surname
	
				FROM reported_issues issue
				INNER JOIN users resident ON issue.resident_id = resident.id 
				WHERE issue.building_id = @building_id AND	
					  issue.resident_id = @resident_id"
            )
			.AddParameter("building_id", buildingId)
			.AddParameter("resident_id", residentId);

			List<Issue> issues = new();

            query.ExecuteReader(
				reader => issues.Add(
					_issueMapper.MapToEntity(reader)
				)
			);

            return issues;
        }

		public int UpdateStatus(long issueId, IssueStatus newStatus)
		{
			var query = PostgresQuery.Create(
				@"UPDATE reported_issues
				  SET status = @new_status
				  WHERE id = @issue_id;"
			)
			.AddParameter("@new_status", newStatus)
			.AddParameter("@issue_id", issueId);

			return query.GetRowsAffectedCount();
		}
    }
}
