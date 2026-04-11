using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain;
using CommunityHub.Application.Helpers.Search;

namespace CommunityHub.Application.Database.Repositories
{
    public class BuildingRepository
    {
        private readonly BuildingMapper _buildingMapper = new BuildingMapper();
        private readonly ImageMapper _imageMapper = new ImageMapper();

        public List<Building> Search(BuildingSearchParameters searchParameters)
        {
            var query = PostgresQuery.Create(
                @"SELECT
                building.id AS building_id,
                building.floor_count,
                
                building_manager.id AS building_manager_user_id,
                building_manager.username AS building_manager_username,
                building_manager.password AS building_manager_password,
                building_manager.name AS building_manager_name,
                building_manager.surname AS building_manager_surname,
                building_manager.user_role AS building_manager_user_role,
                
                address.id AS address_id,
                address.street_number,
                address.neighborhood,
                
                street.id AS street_id,
                street.name AS street_name,

                city.id AS city_id,
                city.name AS city_name,

                country.id AS country_id,
                country.name AS country_name,

                image.id AS image_id,
                image.building_id AS image_building_id,
                image.neighborhood_id AS image_neighborhood_id,
                image.path AS path,

                COUNT(DISTINCT apartment.id) AS apartment_count

                FROM buildings building
                INNER JOIN users building_manager ON building.building_manager_id = building_manager.id
                INNER JOIN addresses address ON building.address_id = address.id
                INNER JOIN streets street ON address.street_id = street.id
                INNER JOIN cities city ON address.city_id = city.id
                INNER JOIN countries country ON city.country_id = country.id
                INNER JOIN apartments apartment ON apartment.building_id = building.id
                LEFT JOIN images image ON image.building_id = building.id

                WHERE
                (country.id = @country_id OR @country_id IS NULL) AND
                (city.id = @city_id OR @city_id IS NULL) AND
                (street.name ILIKE '%' || @street_name || '%') AND
                (address.neighborhood ILIKE '%' || @neighborhood || '%')

                GROUP BY 
                building.id, building.floor_count, building_manager.id, building_manager.username, building_manager.password, building_manager.name, building_manager.surname, building_manager.user_role,
                address.id, address.street_number, address.neighborhood,
                street.id, street.name,
                city.id, city.name,
                country.id, country.name,
                image.id, image.building_id, image.neighborhood_id, image.path"
            ) 
            .ExtendParameters(searchParameters.ToQueryParameters());
            
            Dictionary<long, Building> buildings = new Dictionary<long, Building>();
            
            query.ExecuteReader(reader => {
                Building newBuilding = _buildingMapper.MapToEntity(reader);
                
                if (!buildings.TryGetValue(newBuilding.Id, out Building? existingBuilding))
                {
                    buildings.Add(newBuilding.Id, newBuilding);
                    existingBuilding = newBuilding;
                }

                Image? image = _imageMapper.MapToEntity(reader);
                if (image == null) return;
                existingBuilding.Images.Add(image);
            });


            return buildings.Values.ToList();
        }
        public List<Building> GetByManagerId(long managerId)
        {
            var query = PostgresQuery.Create(
               @"SELECT
                building.id AS building_id,
                building.floor_count,
                
                building_manager.id AS building_manager_user_id,
                building_manager.username AS building_manager_username,
                building_manager.password AS building_manager_password,
                building_manager.name AS building_manager_name,
                building_manager.surname AS building_manager_surname,
                building_manager.user_role AS building_manager_user_role,
                
                address.id AS address_id,
                address.street_number,
                address.neighborhood,
                
                street.id AS street_id,
                street.name AS street_name,

                city.id AS city_id,
                city.name AS city_name,

                country.id AS country_id,
                country.name AS country_name,

                image.id AS image_id,
                image.building_id AS image_building_id,
                image.neighborhood_id AS image_neighborhood_id,
                image.path AS path,

                COUNT(DISTINCT apartment.id) AS apartment_count

                FROM buildings building
                INNER JOIN users building_manager ON building.building_manager_id = building_manager.id
                INNER JOIN addresses address ON building.address_id = address.id
                INNER JOIN streets street ON address.street_id = street.id
                INNER JOIN cities city ON address.city_id = city.id
                INNER JOIN countries country ON city.country_id = country.id
                INNER JOIN apartments apartment ON apartment.building_id = building.id
                LEFT JOIN images image ON image.building_id = building.id

                WHERE
                building_manager.id = @building_manager_id

                GROUP BY 
                building.id, building.floor_count, building_manager.id, building_manager.username, building_manager.password, building_manager.name, building_manager.surname, building_manager.user_role,
                address.id, address.street_number, address.neighborhood,
                street.id, street.name,
                city.id, city.name,
                country.id, country.name,
                image.id, image.building_id, image.neighborhood_id, image.path"
           )
           .AddParameter("building_manager_id", managerId);

            Dictionary<long, Building> buildings = new Dictionary<long, Building>();

            query.ExecuteReader(reader => {
                Building newBuilding = _buildingMapper.MapToEntity(reader);

                if (!buildings.TryGetValue(newBuilding.Id, out Building? existingBuilding))
                {
                    buildings.Add(newBuilding.Id, newBuilding);
                    existingBuilding = newBuilding;
                }

                Image? image = _imageMapper.MapToEntity(reader);
                if (image == null) return;
                existingBuilding.Images.Add(image);
            });

            return buildings.Values.ToList();
        }
    }
}