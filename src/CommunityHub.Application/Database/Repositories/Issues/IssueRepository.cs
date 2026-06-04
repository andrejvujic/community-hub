using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain.Issues;
using CommunityHub.Application.Domain.RepositoryInterfaces.Issues;

namespace CommunityHub.Application.Database.Repositories.Issues
{
	public class IssueRepository : IIssueRepository
	{
		private readonly IssueMapper _issueMapper = new();

		public Issue Create(Issue issue)
		{
			using var connection = DatabaseConnection.Create();

			var query = DatabaseQuery.Create(
				@"INSERT INTO reported_issues
					(description, reported_date, priority, status, resident_id, building_id)
				VALUES
					(@description, @reported_date, @priority, @status, @resident_id, @building_id)
				RETURNING id;"
			).ExtendParameters(_issueMapper.MapToQueryParameters(issue));

			long id = connection.ExecuteScalar(query, Convert.ToInt64);

			return issue.CopyWith(id: id);
		}

		public List<Issue> GetAllByBuilding(long buildingId, DateTime? from = null, DateTime? to = null)
		{
			using var connection = DatabaseConnection.Create();

			var query = DatabaseQuery.Create(
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
					WHERE reported_issue.building_id = @building_id AND
					(@from::date IS NULL OR reported_issue.reported_date >= @from::date) AND
					(@to::date IS NULL OR reported_issue.reported_date <= @to::date)
					ORDER BY reported_issue.reported_date ASC;"
                )
				.AddParameter("@building_id", buildingId)
				.AddParameter("@from", from)
                .AddParameter("@to", to);


            List<Issue> issues = [];

			connection.ExecuteReader(query, reader => issues.Add(_issueMapper.MapToEntity(reader)));

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

		public int Update(Issue issue)
		{
			using var connection = DatabaseConnection.Create();

			var query = DatabaseQuery.Create(
                @"UPDATE reported_issues
				  SET status = @status, description = @description, priority = @priority, reported_date = @reported_date
				  WHERE id = @issue_id;"
            )
			.AddParameter("@status", issue.Status)
			.AddParameter("@description", issue.Description)
			.AddParameter("@priority", issue.Priority)
			.AddParameter("@reported_date", issue.ReportedAt)
			.AddParameter("@issue_id", issue.Id);

			return connection.GetRowsAffectedCount(query);
		}
	}
}
