namespace CommunityHub.Application.Database.Repositories
{
    public class ApartmentMembershipRepository
    {
        public bool IsApartmentOccupied(long apartmentId)
        {
            var query = PostgresQuery.Create(
            @"SELECT
                membership.id
            FROM apartment_memberships membership
            WHERE
                membership.apartment_id = @apartment_id"
            )
            .AddParameter("apartment_id", apartmentId);

            return query.ExecuteScalar<bool>(membershipId => membershipId != null);
        }

        public bool HasMembership(long userId, long apartmentId)
        {
            var query = PostgresQuery.Create(
                @"SELECT
                    membership.id
                FROM apartment_memberships membership
                WHERE
                    membership.apartment_id = @apartment_id AND
                    membership.member_id = @user_id"
            )
            .AddParameter("apartment_id", apartmentId)
            .AddParameter("user_id", userId);

            return query.ExecuteScalar<bool>(membershipId => membershipId != null);
        }
    }
}
