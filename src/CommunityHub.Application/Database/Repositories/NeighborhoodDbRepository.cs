using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain;

namespace CommunityHub.Application.Database.Repositories
{
    public class NeighborhoodDbRepository
    {
        private readonly NeighborhoodMapper _neighborhoodMapper = new NeighborhoodMapper();
        public List<Neighborhood> GetByCoordinatorId(long id)
        {
            var query = PostgresQuery.Create(
                @"SELECT
                neighborhood.id AS neighborhood_id,
                neighborhood.name AS neighborhood_name,
                neighborhood.description,
                neighborhood.budget,

                city.id AS city_id,
                city.name AS city_name,

                country.id AS country_id,
                country.name AS country_name,

                coordinator.id AS coordinator_user_id,
                coordinator.username AS coordinator_username,
                coordinator.password AS coordinator_password,
                coordinator.name AS coordinator_name,
                coordinator.surname AS coordinator_surname,
                coordinator.user_role AS coordinator_user_role

                FROM neighborhoods neighborhood
                INNER JOIN cities city ON neighborhood.city_id = city.id
                INNER JOIN countries country ON city.country_id = country.id
                INNER JOIN users coordinator ON neighborhood.coordinator_id = coordinator.id
                WHERE neighborhood.coordinator_id = @id"
            ).AddParameter("id", id);

            List<Neighborhood> neighborhoods = new List<Neighborhood>();
            query.ExecuteReader(reader => neighborhoods.Add(_neighborhoodMapper.MapToEntity(reader)));

            return neighborhoods;
        }
    }
}
