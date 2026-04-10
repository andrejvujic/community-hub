using CommunityHub.Application.Database.Mapping.EntityMappers;

namespace CommunityHub.Application.Database.Repositories
{
    public class AddressDbRepository
    {
        public List<int> GetOccupiedStreetNumbers(long streetId, long cityId)
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
    }
}
