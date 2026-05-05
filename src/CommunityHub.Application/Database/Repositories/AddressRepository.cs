using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.RepositoryInterfaces;

namespace CommunityHub.Application.Database.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly AddressMapper _addressMapper = new AddressMapper();
        public List<int> GetOccupiedStreetNumbers(long? streetId, long? cityId)
        {
            var query = PostgresQuery.Create(
                @"SELECT street_number FROM
                  buildings INNER JOIN addresses ON
                  buildings.address_id = addresses.id
                  WHERE street_id = @streetId AND city_id = @cityId;"
            )
            .AddParameter("streetId", streetId)
            .AddParameter("cityId", cityId);

            List<int> occupiedStreetNumbers = new List<int>();
           
            query.ExecuteReader(reader =>
            {
                var value = reader.GetValue(0);
                if (value != null && int.TryParse(value.ToString(), out var number))
                    occupiedStreetNumbers.Add(number);
            });

            return occupiedStreetNumbers;
        }

        public Address Create(Address address)
        {
            var query = PostgresQuery.Create(
                @"INSERT INTO addresses 
                (street_number, neighborhood, street_id, city_id) 
                VALUES 
                (@street_number, @neighborhood, @street_id, @city_id)
                RETURNING id"
            )
            .ExtendParameters(_addressMapper.MapToQueryParameters(address));
            long generatedId = query.ExecuteScalar<long>(value => Convert.ToInt64(value));
            return address.CopyWith(id: generatedId);
        }
    }
}
