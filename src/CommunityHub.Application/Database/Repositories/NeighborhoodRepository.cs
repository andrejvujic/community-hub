using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain;

namespace CommunityHub.Application.Database.Repositories
{
    public class NeighborhoodRepository
    {
        private readonly NeighborhoodMapper _neighborhoodMapper = new NeighborhoodMapper();
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
    }
}
