using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain.ReportedIsssue;

namespace CommunityHub.Application.Database.Repositories
{
	public class ReportedIssueRepository
	{
		private readonly ReportedIssueMapper _reportedIssueMapper = new();
		public List<ReportedIssue> GetAllByBuilding(long buildingId)
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

			List<ReportedIssue> reportedIssues = new List<ReportedIssue>();

			query.ExecuteReader(reader => reportedIssues.Add(_reportedIssueMapper.MapToEntity(reader)));

			return reportedIssues;
		}

		public int UpdateStatus(long issueId, ReportedIssueStatus newStatus)
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
