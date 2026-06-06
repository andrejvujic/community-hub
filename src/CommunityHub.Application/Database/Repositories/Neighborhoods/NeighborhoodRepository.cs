using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.Neighborhoods;
using CommunityHub.Application.Domain.RepositoryInterfaces.Neighborhoods;
using CommunityHub.Application.Helpers.Search;

namespace CommunityHub.Application.Database.Repositories
{
    public class NeighborhoodRepository : INeighborhoodRepository
    {
        private readonly NeighborhoodMapper _neighborhoodMapper = new NeighborhoodMapper();
        private readonly ImageMapper _imageMapper = new ImageMapper();

        public List<Neighborhood> GetByCoordinatorId(long id)
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery.Create(
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
                coordinator.user_role AS coordinator_user_role,

                image.id AS image_id,
                image.building_id AS image_building_id,
                image.neighborhood_id AS image_neighborhood_id,
                image.job_listing_id AS image_job_listing_id,
                image.path AS path

                FROM neighborhoods neighborhood
                INNER JOIN cities city ON neighborhood.city_id = city.id
                INNER JOIN countries country ON city.country_id = country.id
                INNER JOIN users coordinator ON neighborhood.coordinator_id = coordinator.id
                LEFT JOIN images image ON image.neighborhood_id = neighborhood.id
                WHERE neighborhood.coordinator_id = @id"
            ).AddParameter("id", id);



            Dictionary<long, Neighborhood> neighborhoods = new Dictionary<long, Neighborhood>();

            connection.ExecuteReader(query, reader => {
                Neighborhood newNeighborhood = _neighborhoodMapper.MapToEntity(reader);
                if (!neighborhoods.TryGetValue(newNeighborhood.Id, out Neighborhood? existingNeighborhood))
                {
                    neighborhoods.Add(newNeighborhood.Id, newNeighborhood);
                    existingNeighborhood = newNeighborhood;
                }

                Image? image = _imageMapper.MapToEntity(reader);
                if (image == null) return;
                existingNeighborhood.Images.Add(image);
            });

            return neighborhoods.Values.ToList();
        }


        public Neighborhood Create(Neighborhood neighborhood)
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery.Create(
                @"INSERT INTO neighborhoods 
                (name, description, budget, city_id, coordinator_id) 
                VALUES 
                (@name, @description, @budget, @city_id, @coordinator_id) 
                RETURNING id"
            )
            .ExtendParameters(_neighborhoodMapper.MapToQueryParameters(neighborhood));

            long neighborhoodId = connection.ExecuteScalar(query, Convert.ToInt64);

            return neighborhood.CopyWith(id: neighborhoodId);
        }

        public List<Neighborhood> Search(NeighborhoodSearchParameters searchParameters)
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery.Create(
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
                coordinator.user_role AS coordinator_user_role,

                image.id AS image_id,
                image.building_id AS image_building_id,
                image.neighborhood_id AS image_neighborhood_id,
                image.job_listing_id AS image_job_listing_id,
                image.path AS path

                FROM neighborhoods neighborhood
                INNER JOIN cities city ON neighborhood.city_id = city.id
                INNER JOIN countries country ON city.country_id = country.id
                INNER JOIN users coordinator ON neighborhood.coordinator_id = coordinator.id
                LEFT JOIN street_sections section ON neighborhood.id = section.neighborhood_id
                LEFT JOIN streets street ON section.street_id = street.id
                LEFT JOIN images image ON image.neighborhood_id = neighborhood.id

                WHERE 
                (country.id = @country_id OR @country_id IS NULL) AND
                (city.id = @city_id OR @city_id IS NULL) AND
                (neighborhood.name ILIKE '%' || @name || '%') AND
                (CAST(@street_name AS text) IS NULL OR CAST(@street_name AS text) = '' OR street.name ILIKE '%' || CAST(@street_name AS text) || '%') AND 
                (CAST(@street_number AS int) IS NULL OR CAST(@street_number AS int) BETWEEN section.start_number AND section.end_number)

                GROUP BY 
                neighborhood.id, neighborhood.name, neighborhood.description, neighborhood.budget,
                city.id, city.name,
                country.id, country.name,
                coordinator.id, coordinator.username, coordinator.password, coordinator.name, coordinator.surname, coordinator.user_role,
                image.id, image.building_id, image.neighborhood_id, image.path"
            )
            .ExtendParameters(searchParameters.ToQueryParameters());

            Dictionary<long, Neighborhood> neighborhoods = new Dictionary<long, Neighborhood>();

            connection.ExecuteReader(query, reader => {
                Neighborhood newNeighborhood = _neighborhoodMapper.MapToEntity(reader);

                if (!neighborhoods.TryGetValue(newNeighborhood.Id, out Neighborhood? existingNeighborhood))
                {
                    neighborhoods.Add(newNeighborhood.Id, newNeighborhood);
                    existingNeighborhood = newNeighborhood;
                }

                Image? image = _imageMapper.MapToEntity(reader);
                if (image != null)
                {
                    existingNeighborhood.Images.Add(image);
                }
            });

            return neighborhoods.Values.ToList();
        }

        public void UpdateBudget(long neighborhoodId, decimal newBudget)
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery.Create(
                @"UPDATE neighborhoods
                SET budget = @budget
                WHERE id = @id"
            )
            .AddParameter("id", neighborhoodId)
            .AddParameter("budget", newBudget);

            connection.Execute(query);
        }

        public int GetNeighborhoodCountsByCoordinatorId(long coordinatorId)
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery.Create(
                @"SELECT 
                    COUNT(DISTINCT n.id) AS neighborhood_count
                FROM neighborhoods n
                LEFT JOIN neighborhood_memberships nm ON nm.neighborhood_id = n.id
                LEFT JOIN citizens m ON m.membership_id = nm.id
                WHERE n.coordinator_id = @coordinator_id"
            ).AddParameter("coordinator_id", coordinatorId);

            int neighborhoodCount = 0;
            connection.ExecuteReader(query, reader =>
            {
                neighborhoodCount = reader.GetInt32(reader.GetOrdinal("neighborhood_count"));
            });
            return neighborhoodCount;
        }
    }
}
