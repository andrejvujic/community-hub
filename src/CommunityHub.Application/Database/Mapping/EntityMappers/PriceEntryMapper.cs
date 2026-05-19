using CommunityHub.Application.Domain.JobListings;
using System.Data;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class PriceEntryMapper : EntityMapper<PriceEntry>
    {
        public override PriceEntry? MapToEntity(IDataReader reader)
        {
            int idOrdinal = reader.GetOrdinal("price_entry_id");
            if (reader.IsDBNull(idOrdinal)) return null;
            long id = reader.GetInt64(idOrdinal);

            decimal amount = reader.GetDecimal(reader.GetOrdinal("amount"));
            DateOnly validFrom = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("valid_from")));
            DateOnly validTo = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("valid_to")));
            long jobListingId = reader.GetInt64(reader.GetOrdinal("price_entry_listing_id"));

            return new PriceEntry(amount, validFrom, validTo, jobListingId, id);
        }

        public override Dictionary<string, object?> MapToQueryParameters(PriceEntry entity)
        {
            return new Dictionary<string, object?>
            {
                { "amount", entity.Amount },
                { "valid_from", entity.ValidFrom.ToDateTime(TimeOnly.MinValue) },
                { "valid_to", entity.ValidTo.ToDateTime(TimeOnly.MinValue) },
                { "job_listing_id", entity.JobListingId }
            };
        }
    }
}