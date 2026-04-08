using CommunityHub.Application.Database.EntityMappers;
using CommunityHub.Application.Domain;
using CommunityHub.Application.Helpers.Search;
using System.Data;
using System.Diagnostics;

namespace CommunityHub.Application.Database.Repositories
{
    public class BuildingRepository
    {
        private readonly BuildingMapper _buildingMapper = new BuildingMapper();
        public List<Building> Search(BuildingSearchParameters searchParameters)
        {
            var query = PostgresQuery.Create(
                @"SELECT
                building.id as building_id,
                building.floor_count,
                
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

                FROM buildings building
                INNER JOIN users building_manager ON building.building_manager_id = building_manager.id
                INNER JOIN addresses address ON building.address_id = address.id
                INNER JOIN streets street ON address.street_id = street.id
                INNER JOIN cities city ON address.city_id = city.id
                INNER JOIN countries country ON city.country_id = country.id

                WHERE
                (country.id = @country_id OR @country_id = 0) AND
                (city.id = @city_id OR @city_id = 0) AND
                (street.name ILIKE '%' || @street_name || '%') AND
                (address.neighborhood ILIKE '%' || @neighborhood || '%')
                "
            ) 
            .ExtendParameters(searchParameters.ToQueryParameters());

            List<Building> buildings = new List<Building>();
            
            query.ExecuteReader((reader) => {
                buildings.Add(_buildingMapper.MapToEntity(reader));
            });


            return buildings;
        }
    }
}
