using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain.Apartments;
using CommunityHub.Application.Domain.RepositoryInterfaces.Apartments;

namespace CommunityHub.Application.Database.Repositories.Apartments;

public class ApartmentRepository : IApartmentRepository
{
    private readonly ApartmentMapper _apartmentMapper = new();

    public Apartment Create(Apartment apartment)
    {
        using var connection = DatabaseConnection.Create();

        var query = DatabaseQuery.Create(
            @"INSERT INTO apartments (apartment_number, floor_number, building_id) 
                VALUES (@apartment_number, @floor_number, @building_id) 
                RETURNING id"
        )
        .ExtendParameters(_apartmentMapper.MapToQueryParameters(apartment));

        long id = connection.ExecuteScalar(query, id => Convert.ToInt64(id));

        return apartment.CopyWith(id: id);
    }

    public Apartment? Get(long id)
    {
        var query = PostgresQuery.Create(
            @"SELECT
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

        query.ExecuteReader(
            reader => apartment = _apartmentMapper.MapToEntity(reader)
        );

        return apartment;
    }

    public long? FindIdByBuildingAndNumber(long buildingId, int apartmentNumber)
    {
        using var connection = DatabaseConnection.Create();

        var query = DatabaseQuery.Create(
            @"SELECT
                apartment.id
            FROM apartments apartment
            WHERE
                apartment.building_id = @building_id AND
                apartment.apartment_number = @apartment_number"
        )
        .AddParameter("building_id", buildingId)
        .AddParameter("apartment_number", apartmentNumber);

        return connection.ExecuteScalar<long?>(
            query,
            id => Convert.ToInt64(id)
        );
    }

    public bool Exists(long buildingId, int apartmentNumber)
    {
        using var connection = DatabaseConnection.Create();

        var query = DatabaseQuery.Create(
            @"SELECT EXISTS(
            SELECT 1
            FROM apartments apartment
            WHERE
                apartment.building_id = @building_id AND
                apartment.apartment_number = @apartment_number)"
        )
        .AddParameter("building_id", buildingId)
        .AddParameter("apartment_number", apartmentNumber);

        return connection.ExecuteScalar(query, Convert.ToBoolean);
    }
}
