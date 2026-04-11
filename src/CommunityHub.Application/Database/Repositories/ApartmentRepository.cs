using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain;

namespace CommunityHub.Application.Database.Repositories;

public class ApartmentRepository
{
    private readonly ApartmentMapper _apartmentMapper = new ApartmentMapper();

    public Apartment? GetById(long id)
    {
        var query = PostgresQuery.Create(
            @"SELECT
                apartment.id as apartment_id,
                apartment.apartment_number,
                apartment.floor_number,
                
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

            FROM apartments apartment
                INNER JOIN buildings building ON apartment.building_id = building.id
                INNER JOIN users building_manager ON building.building_manager_id = building_manager.id
                INNER JOIN addresses address ON building.address_id = address.id
                INNER JOIN streets street ON address.street_id = street.id
                INNER JOIN cities city ON address.city_id = city.id
                INNER JOIN countries country ON city.country_id = country.id
            
            WHERE apartment.id = @id"
            ).AddParameter("id", id);
        
        Apartment? apartment = null;
        query.ExecuteReader(reader => apartment = _apartmentMapper.MapToEntity(reader));
        return apartment;
    }

    public long? FindIdByBuildingAndNumber(long buildingId, int apartmentNumber)
    {
        var query = PostgresQuery.Create(
            @"SELECT
                apartment.id
            FROM apartments apartment
            WHERE
                apartment.building_id = @building_id AND
                apartment.apartment_number = @apartment_number"
        )
        .AddParameter("building_id", buildingId)
        .AddParameter("apartment_number", apartmentNumber);

        return query.ExecuteScalar<long?>(apartmentId => Convert.ToInt64(apartmentId));
    }

    public List<Apartment> GetAllByBuildingId(long id)
    {
        var query = PostgresQuery.Create(
            @"SELECT
                apartment.id AS apartment_id,
                apartment.apartment_number,
                apartment.floor_number,
                
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
                country.name AS country_name

                FROM apartments apartment
                INNER JOIN buildings building ON apartment.building_id = building.id
                INNER JOIN users building_manager ON building.building_manager_id = building_manager.id
                INNER JOIN addresses address ON building.address_id = address.id
                INNER JOIN streets street ON address.street_id = street.id
                INNER JOIN cities city ON address.city_id = city.id
                INNER JOIN countries country ON city.country_id = country.id
                WHERE apartment.building_id = @id"
        ).AddParameter("id", id);

        List<Apartment> apartments = new List<Apartment>();
        query.ExecuteReader(reader => {
            Apartment? apartment = _apartmentMapper.MapToEntity(reader);
            if (apartment == null) return;
            apartments.Add(apartment);
        });
        return apartments;
    }


    public bool Exists(long apartmentId)
    {
        var query = PostgresQuery.Create(
            @"SELECT
                apartment.id
            FROM apartments apartment
            WHERE apartment.id = @apartment_id"
            )
            .AddParameter("apartment_id", apartmentId);

        return query.ExecuteScalar<bool>(apartmentId => apartmentId != null);
    }
}
