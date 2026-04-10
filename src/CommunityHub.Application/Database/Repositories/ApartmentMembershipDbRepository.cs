using CommunityHub.Application.Domain;

namespace CommunityHub.Application.Database.Repositories
{
    public class ApartmentMembershipDbRepository
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

        public bool IsResidentOfApartment(long residentId, long apartmentId)
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
                resident.id = @resident_id"
            )
            .AddParameter("apartment_id", apartmentId)
            .AddParameter("resident_id", residentId);

            return query.ExecuteScalar<bool>(membershipId => membershipId != null);
        }
    }
}
