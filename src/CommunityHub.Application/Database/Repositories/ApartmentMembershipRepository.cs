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
            INNER JOIN apartments apartment
            ON membership.apartment_id = apartment.id
            WHERE
                apartment.id = @apartment_id"
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
            INNER JOIN apartments apartment
            ON membership.apartment_id = apartment.id
            INNER JOIN users resident
            ON membership.building_resident_id = resident.id
            WHERE
                apartment.id = @apartment_id AND
                resident.id = @user_id"
            )
            .AddParameter("apartment_id", apartmentId)
            .AddParameter("user_id", userId);

            return query.ExecuteScalar<bool>(membershipId => membershipId != null);
        }
    }
}
