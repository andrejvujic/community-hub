using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain.Neighborhoods.CityFacilities;

namespace CommunityHub.Application.Database.Repositories.Neighborhoods.CityFacilities
{
    public class FacilityReservationRepository
    {
        private readonly FacilityReservationMapper facilityReservationMapper = new FacilityReservationMapper();

        private const string SelectColumns = @"
            facility_reservation.id AS facility_reservation_id,
            facility_reservation.start_datetime,
            facility_reservation.end_datetime,

            city_facility.id AS city_facility_id,
            city_facility.name AS city_facility_name,

            facility_city.id AS facility_city_id,
            facility_city.name AS facility_city_name,

            facility_country.id AS facility_country_id,
            facility_country.name AS facility_country_name,

            neighborhood.id AS neighborhood_id,
            neighborhood.name AS neighborhood_name,
            neighborhood.description,
            neighborhood.budget,

            neighborhood_city.id AS city_id,
            neighborhood_city.name AS city_name,

            neighborhood_country.id AS country_id,
            neighborhood_country.name AS country_name,

            coordinator.id AS coordinator_user_id,
            coordinator.username AS coordinator_username,
            coordinator.password AS coordinator_password,
            coordinator.name AS coordinator_name,
            coordinator.surname AS coordinator_surname,
            coordinator.user_role AS coordinator_user_role";

        private const string FromJoins = @"
            FROM facility_reservations facility_reservation
            INNER JOIN city_facilities city_facility ON city_facility.id = facility_reservation.facility_id
            INNER JOIN cities facility_city ON facility_city.id = city_facility.city_id
            INNER JOIN countries facility_country ON facility_country.id = facility_city.country_id
            INNER JOIN neighborhoods neighborhood ON neighborhood.id = facility_reservation.neighborhood_id
            INNER JOIN cities neighborhood_city ON neighborhood_city.id = neighborhood.city_id
            INNER JOIN countries neighborhood_country ON neighborhood_country.id = neighborhood_city.country_id
            INNER JOIN users coordinator ON coordinator.id = neighborhood.coordinator_id";

        public List<FacilityReservation> GetByNeighborhoodId(long neighborhoodId)
        {
            var query = PostgresQuery.Create(
                $"SELECT {SelectColumns} {FromJoins} WHERE neighborhood.id = @neighborhood_id ORDER BY facility_reservation.start_datetime DESC"
            ).AddParameter("neighborhood_id", neighborhoodId);
            
            List<FacilityReservation> reservations = new List<FacilityReservation>();
            query.ExecuteReader(reader =>
            {
                var reservation = facilityReservationMapper.MapToEntity(reader);
                if (reservation is not null) reservations.Add(reservation);
            });
            return reservations;
        }

        public List<FacilityReservation> GetByFacilityId(long facilityId)
        {
            var query = PostgresQuery.Create(
                $"SELECT {SelectColumns} {FromJoins} WHERE facility_reservation.facility_id = @facility_id ORDER BY facility_reservation.start_datetime"
            ).AddParameter("facility_id", facilityId);

            List<FacilityReservation> reservations = new List<FacilityReservation>();
            query.ExecuteReader(reader =>
            {
                var reservation = facilityReservationMapper.MapToEntity(reader);
                if (reservation is not null) reservations.Add(reservation);
            });
            return reservations;
        }

        public FacilityReservation Create(FacilityReservation reservation)
        {
            var query = PostgresQuery.Create(
                @"INSERT INTO facility_reservations (start_datetime, end_datetime, facility_id, neighborhood_id)
                VALUES (@start_datetime, @end_datetime, @facility_id, @neighborhood_id)
                RETURNING id"
            )
            .ExtendParameters(facilityReservationMapper.MapToQueryParameters(reservation));
            
            long id = query.ExecuteScalar<long>(value => Convert.ToInt64(value));

            return reservation.CopyWith(id: id);
        }
    }
}
