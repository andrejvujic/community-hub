using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain.Addresses;
using CommunityHub.Application.Domain.Addresses.RepositoryInterfaces;

namespace CommunityHub.Application.Database.Repositories.Shared.Addresses
{
    public class AddressRepository : IAddressRepository
    {
        private readonly AddressMapper _addressMapper = new();
        public List<int> GetOccupiedStreetNumbers(long? streetId, long? cityId)
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery.Create(
                @"SELECT street_number FROM
                  buildings INNER JOIN addresses ON
                  buildings.address_id = addresses.id
                  WHERE street_id = @streetId AND city_id = @cityId;"
            )
            .AddParameter("streetId", streetId)
            .AddParameter("cityId", cityId);

            List<int> occupiedStreetNumbers = [];
           
            connection.ExecuteReader(query, reader =>
            {
                var value = reader.GetValue(0);
                if (value != null && int.TryParse(value.ToString(), out var number))
                    occupiedStreetNumbers.Add(number);
            });

            return occupiedStreetNumbers;
        }

        public Address Create(Address address)
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery.Create(
                @"INSERT INTO addresses 
                (street_number, neighborhood, street_id, city_id) 
                VALUES 
                (@street_number, @neighborhood, @street_id, @city_id)
                RETURNING id"
            )
            .ExtendParameters(_addressMapper.MapToQueryParameters(address));

            long generatedId = connection.ExecuteScalar(query, value => Convert.ToInt64(value));
            return address.CopyWith(id: generatedId);
        }
    }
}
