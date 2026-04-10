namespace CommunityHub.Application.Database.Repositories
{
    public class ApartmentMembershipDbRepository
    {
        public bool IsApartmentOccupied(long buildingId, int apartmentNumber)
        {
            var query = PostgresQuery.Create(
            @"SELECT
                membership.id AS membership_id
            FROM apartment_memberships membership
            INNER JOIN apartments apartment
            ON membership.apartment_id = apartment.id
            WHERE
                apartment.building_id = @building_id AND
                apartment.apartment_number = @apartment_number"
            )
            .AddParameter("building_id", buildingId)
            .AddParameter("apartment_number", apartmentNumber);

            return query.ExecuteScalar<bool>(membershipId => membershipId != null);
        }
    }
}
