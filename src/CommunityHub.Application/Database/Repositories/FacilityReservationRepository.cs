namespace CommunityHub.Application.Database.Repositories
{
    public class FacilityReservationRepository
    {
        private const string SelectColumns = @"
            facility_reservation.id AS facility_reservation_id,
            facility_reservation.start_datetime,
            facility_reservation.end_datetime,

            city_facility.id AS city_facility_id,
            city_facility.name AS city_facility_name,

            facility_city.id AS city_id,
            facility_city.name AS city_name,

            facility_country.id AS country_id,
            facility_country.name AS country_name,

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
    }
}
