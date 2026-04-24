using System.Net.NetworkInformation;
using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain;

namespace CommunityHub.Application.Database.Repositories
{
    public class PriceEntryRepository
    {
        private readonly PriceEntryMapper _priceEntryMapper = new PriceEntryMapper();

        public List<PriceEntry> GetByJobListingId(long jobListingId)
        {
            var query = PostgresQuery.Create(
                @"SELECT
                    price_entry.id AS price_entry_id,
                    price_entry.amount,
                    price_entry.valid_from,
                    price_entry.valid_to,
                    price_entry.job_listing_id AS price_entry_listing_id
                FROM price_entries price_entry
                WHERE price_entry.job_listing_id = @job_listing_id
                ORDER BY price_entry.valid_from"
            ).AddParameter("job_listing_id", jobListingId);

            var results = new List<PriceEntry>();
            List<PriceEntry> priceEntries = new List<PriceEntry>();
            query.ExecuteReader(reader =>{
                priceEntries.Add(_priceEntryMapper.MapToEntity(reader));
            });

            return priceEntries;
        }
    }
}
