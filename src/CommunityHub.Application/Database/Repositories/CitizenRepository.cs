namespace CommunityHub.Application.Database.Repositories
{
    public class CitizenRepository
    {
        public void UpdateMembership(long citizenId, long membershipId)
        {
            var query = PostgresQuery.Create(
                "UPDATE citizens SET membership_id = @membershipId WHERE id = @citizenId"
            )
            .AddParameter("membershipId", membershipId)
            .AddParameter("citizenId", citizenId);

            query.GetRowsAffectedCount();
        }
    }
}
