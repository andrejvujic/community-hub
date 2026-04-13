using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain;
using CommunityHub.Application.Helpers.Search;

namespace CommunityHub.Application.Database.Repositories
{
    public class NeighborhoodRepository
    {
        private readonly NeighborhoodMapper _neighborhoodMapper = new NeighborhoodMapper();
        private readonly ImageMapper _imageMapper = new ImageMapper();
        private readonly StreetSectionRepository _streetSectionRepository = new StreetSectionRepository();
        private readonly ImageRepository _imageRepository = new ImageRepository();

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

        public Neighborhood Create(Neighborhood neighborhood)
        {
            var query = PostgresQuery.Create(
                @"INSERT INTO neighborhoods 
                (name, description, budget, city_id, coordinator_id) 
                VALUES 
                (@name, @description, @budget, @city_id, @coordinator_id) 
                RETURNING id"
            )
            .ExtendParameters(_neighborhoodMapper.MapToQueryParameters(neighborhood));

            long neighborhoodId = query.ExecuteScalar<long>(value => Convert.ToInt64(value));

            List<StreetSection> savedSections = new List<StreetSection>();
            foreach (var section in neighborhood.StreetSections)
            {
                var savedSection = _streetSectionRepository.Create(section, neighborhoodId);
                savedSections.Add(savedSection);
            }

            List<Image> savedImages = new List<Image>();
            foreach (var image in neighborhood.Images)
            {
                var savedImage = _imageRepository.Create(image, neighborhoodId: neighborhoodId);
                savedImages.Add(savedImage);
            }

            return neighborhood.CopyWith(
                id: neighborhoodId,
                streetSections: savedSections,
                images: savedImages
            );
        }

        public List<Neighborhood> Search(NeighborhoodSearchParameters searchParameters)
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
                coordinator.user_role AS coordinator_user_role,

                image.id AS image_id,
                image.building_id AS image_building_id,
                image.neighborhood_id AS image_neighborhood_id,
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

            query.ExecuteReader(reader => {
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
    }
}
