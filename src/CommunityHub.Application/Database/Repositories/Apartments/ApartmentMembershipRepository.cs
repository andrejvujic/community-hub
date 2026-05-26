using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain.Apartments;
using CommunityHub.Application.Domain.RepositoryInterfaces.Apartments;

namespace CommunityHub.Application.Database.Repositories.Apartments
{
    public class ApartmentMembershipRepository : IApartmentMembershipRepository
    {
        private readonly ApartmentMembershipMapper _apartmentMembershipMapper = new();

        public ApartmentMembership Create(ApartmentMembership membership)
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery.Create(
                @"INSERT INTO apartment_memberships
                (building_resident_id, apartment_id, acceptance_date)
                VALUES
                (@resident_id, @apartment_id, @acceptance_date)
                RETURNING id"
            ).ExtendParameters(
                _apartmentMembershipMapper.MapToQueryParameters(membership)
            );

            long id = connection.ExecuteScalar(query, Convert.ToInt64);
            return membership.CopyWith(id: id);
        }

        public bool IsOccupied(long apartmentId)
        {
            var query = PostgresQuery.Create(
            @"SELECT EXISTS(
                SELECT 1
                FROM apartment_memberships membership
                WHERE membership.apartment_id = @apartment_id)"
            )
            .AddParameter("apartment_id", apartmentId);

            return query.ExecuteScalar(Convert.ToBoolean);
        }

        public List<ApartmentMembership> GetByResident(long id)
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery.Create(
                @"SELECT
                    membership.id as membership_id,
                    membership.acceptance_date,

                    resident.id as resident_user_id,
                    resident.username as resident_username,
                    resident.password as resident_password,
                    resident.name as resident_name,
                    resident.surname as resident_surname,
                    resident.user_role as resident_user_role,

                    apartment.id as apartment_id,
                    apartment.apartment_number,
                    apartment.floor_number,

                    building.id as building_id,
                    building.floor_count,
                    building.apartment_count,

                    building_manager.id as building_manager_user_id,
                    building_manager.username as building_manager_username,
                    building_manager.password as building_manager_password,
                    building_manager.name as building_manager_name,
                    building_manager.surname as building_manager_surname,
                    building_manager.user_role as building_manager_user_role,

                    address.id as address_id,
                    address.street_number,
                    address.neighborhood,

                    street.id as street_id,
                    street.name as street_name,

                    city.id as city_id,
                    city.name as city_name,

                    country.id as country_id,
                    country.name as country_name

                FROM apartment_memberships membership
                INNER JOIN users resident ON membership.building_resident_id = resident.id
                INNER JOIN apartments apartment ON membership.apartment_id = apartment.id
                INNER JOIN buildings building ON apartment.building_id = building.id
                INNER JOIN users building_manager ON building.building_manager_id = building_manager.id
                INNER JOIN addresses address ON building.address_id = address.id
                INNER JOIN streets street ON address.street_id = street.id
                INNER JOIN cities city ON address.city_id = city.id
                INNER JOIN countries country ON city.country_id = country.id
                
                WHERE resident.id = @id"
            ).AddParameter("id", id);

            var memberships = new List<ApartmentMembership>();

            connection.ExecuteReader(
                query,
                reader => memberships.Add(
                    _apartmentMembershipMapper.MapToEntity(reader)
                )
            );

            return memberships;
        }

        public List<ApartmentMembership> GetByApartment(long id)
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery.Create(
                @"SELECT
                    membership.id as membership_id,
                    membership.acceptance_date,

                    resident.id as resident_user_id,
                    resident.username as resident_username,
                    resident.password as resident_password,
                    resident.name as resident_name,
                    resident.surname as resident_surname,
                    resident.user_role as resident_user_role,

                    apartment.id as apartment_id,
                    apartment.apartment_number,
                    apartment.floor_number,

                    building.id as building_id,
                    building.floor_count,
                    building.apartment_count,

                    building_manager.id as building_manager_user_id,
                    building_manager.username as building_manager_username,
                    building_manager.password as building_manager_password,
                    building_manager.name as building_manager_name,
                    building_manager.surname as building_manager_surname,
                    building_manager.user_role as building_manager_user_role,

                    address.id as address_id,
                    address.street_number,
                    address.neighborhood,

                    street.id as street_id,
                    street.name as street_name,

                    city.id as city_id,
                    city.name as city_name,

                    country.id as country_id,
                    country.name as country_name

                FROM apartment_memberships membership
                INNER JOIN users resident ON membership.building_resident_id = resident.id
                INNER JOIN apartments apartment ON membership.apartment_id = apartment.id
                INNER JOIN buildings building ON apartment.building_id = building.id
                INNER JOIN users building_manager ON building.building_manager_id = building_manager.id
                INNER JOIN addresses address ON building.address_id = address.id
                INNER JOIN streets street ON address.street_id = street.id
                INNER JOIN cities city ON address.city_id = city.id
                INNER JOIN countries country ON city.country_id = country.id
                
                WHERE apartment.id = @id"
            )
            .AddParameter("id", id);

            var memberships = new List<ApartmentMembership>();

            connection.ExecuteReader(
                query,
                reader => memberships.Add(
                    _apartmentMembershipMapper.MapToEntity(reader)
                )
            );

            return memberships;
        }

        public List<ApartmentMembership> GetByUserAndBuilding(long userId, long buildingId)
        {
            using var connection = DatabaseConnection.Create();
            
            var query = DatabaseQuery.Create(
                @"SELECT
                membership.id as membership_id,
                membership.acceptance_date,

                resident.id as resident_user_id,
                resident.username as resident_username,
                resident.password as resident_password,
                resident.name as resident_name,
                resident.surname as resident_surname,
                resident.user_role as resident_user_role,

                apartment.id as apartment_id,
                apartment.apartment_number,
                apartment.floor_number,

                building.id as building_id,
                building.floor_count,
                building.apartment_count,

                building_manager.id as building_manager_user_id,
                building_manager.username as building_manager_username,
                building_manager.password as building_manager_password,
                building_manager.name as building_manager_name,
                building_manager.surname as building_manager_surname,
                building_manager.user_role as building_manager_user_role,

                address.id as address_id,
                address.street_number,
                address.neighborhood,

                street.id as street_id,
                street.name as street_name,

                city.id as city_id,
                city.name as city_name,

                country.id as country_id,
                country.name as country_name

                FROM apartment_memberships membership
                INNER JOIN users resident ON membership.building_resident_id = resident.id
                INNER JOIN apartments apartment ON membership.apartment_id = apartment.id
                INNER JOIN buildings building ON apartment.building_id = building.id
                INNER JOIN users building_manager ON building.building_manager_id = building_manager.id
                INNER JOIN addresses address ON building.address_id = address.id
                INNER JOIN streets street ON address.street_id = street.id
                INNER JOIN cities city ON address.city_id = city.id
                INNER JOIN countries country ON city.country_id = country.id
                WHERE building_manager.id = @userId AND building.id = @buildingId;"
            )
            .AddParameter("userId", userId)
            .AddParameter("buildingId", buildingId);

            List<ApartmentMembership> memberships = new();

            connection.ExecuteReader(query, reader =>
            {
                memberships.Add(
                    _apartmentMembershipMapper.MapToEntity(reader)
                );
            });

            return memberships;
        }

        public List<long> GetApartmentIdsByResidentAndBuilding(long residentId, long buildingId)
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery.Create(
                @"SELECT membership.apartment_id
                FROM apartment_memberships membership
                INNER JOIN apartments apartment ON membership.apartment_id = apartment.id
                WHERE membership.building_resident_id = @resident_id
                    AND apartment.building_id = @building_id;"
            )
            .AddParameter("resident_id", residentId)
            .AddParameter("building_id", buildingId);

            List<long> apartmentIds = new();

            connection.ExecuteReader(
                query,
                reader => apartmentIds.Add(
                    reader.GetInt64(
                        reader.GetOrdinal("apartment_id")
                    )
                )
            );

            return apartmentIds;
        }

        public bool Exists(long apartmentId)
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery.Create(
                @"SELECT EXISTS(
                SELECT 1
                FROM apartment_memberships membership
                WHERE membership.apartment_id = @apartment_id)"
            )
            .AddParameter("apartment_id", apartmentId);

            return connection.ExecuteScalar(query, Convert.ToBoolean);
        }

        public bool Exists(long residentId, long apartmentId)
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery.Create(
                @"SELECT EXISTS(
                    SELECT 1
                    FROM apartment_memberships membership
                    WHERE
                        membership.apartment_id = @apartment_id AND
                        membership.building_resident_id = @resident_id)"
            )
            .AddParameter("apartment_id", apartmentId)
            .AddParameter("resident_id", residentId);

            return connection.ExecuteScalar(query, Convert.ToBoolean);
        }
    }
}
