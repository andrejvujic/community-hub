using CommunityHub.Application.Domain;
using System.Data;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    /// <summary>
    /// Maps a single <see cref="BookedJob"/> row.
    /// <para>
    /// Expected column aliases in the query:
    /// <list type="bullet">
    ///   <item><description>
    ///     All <see cref="JobListing"/> columns as documented in <see cref="JobListingMapper"/>.
    ///   </description></item>
    ///   <item><description>
    ///     All <see cref="Neighborhood"/> columns as documented in <see cref="NeighborhoodMapper"/>.
    ///   </description></item>
    ///   <item><description>
    ///     <c>booked_job_id</c>, <c>scheduled_date</c>, <c>scheduled_time</c>,
    ///     <c>duration_hours</c>, <c>unit_price</c>, <c>is_substitution</c>.
    ///   </description></item>
    ///   <item><description>
    ///     Optional LEFT JOIN for the substituting provider, with all user and city
    ///     columns prefixed <c>sub_provider_</c>
    ///     (e.g. <c>sub_provider_user_id</c>, <c>sub_provider_username</c>, …,
    ///     <c>sub_provider_provider_city_id</c>, <c>sub_provider_provider_city_name</c>).
    ///   </description></item>
    /// </list>
    /// </para>
    /// </summary>
    public class BookedJobMapper : EntityMapper<BookedJob>
    {
        private readonly JobListingMapper _listingMapper = new JobListingMapper();
        private readonly NeighborhoodMapper _neighborhoodMapper = new NeighborhoodMapper();
        private readonly ServiceProviderMapper _providerMapper = new ServiceProviderMapper();

        public override BookedJob? MapToEntity(IDataReader reader)
        {
            int idOrdinal = reader.GetOrdinal("booked_job_id");
            if (reader.IsDBNull(idOrdinal)) return null;
            long id = reader.GetInt64(idOrdinal);

            JobListing? listing = _listingMapper.MapToEntity(reader);
            Neighborhood? neighborhood = _neighborhoodMapper.MapToEntity(reader);
            if (listing is null || neighborhood is null) return null;

            DateOnly scheduledDate = DateOnly.FromDateTime(
                reader.GetDateTime(reader.GetOrdinal("scheduled_date")));

            int timeOrdinal = reader.GetOrdinal("scheduled_time");
            TimeOnly? scheduledTime = reader.IsDBNull(timeOrdinal)
                ? null
                : TimeOnly.FromTimeSpan(reader.GetDateTime(timeOrdinal).TimeOfDay);

            int durationOrdinal = reader.GetOrdinal("duration_hours");
            int? durationHours = reader.IsDBNull(durationOrdinal)
                ? null
                : reader.GetInt32(durationOrdinal);

            decimal unitPrice = reader.GetDecimal(reader.GetOrdinal("unit_price"));
            bool isSubstitution = reader.GetBoolean(reader.GetOrdinal("is_substitution"));

            // The substituting provider's columns are prefixed "sub_provider_" in the query
            // so they don't collide with the original provider's columns from the listing join.
            ServiceProvider? substitutingProvider = null;
            if (isSubstitution && reader.HasColumn("sub_provider_user_id"))
            {
                int subIdOrdinal = reader.GetOrdinal("sub_provider_user_id");
                if (!reader.IsDBNull(subIdOrdinal))
                    substitutingProvider = _providerMapper.MapToEntity(
                        reader.WithPrefix("sub_provider"));
            }

            return new BookedJob(
                listing,
                neighborhood,
                scheduledDate,
                unitPrice,
                id: id,
                scheduledTime: scheduledTime,
                durationHours: durationHours,
                isSubstitution: isSubstitution,
                substitutingProvider: substitutingProvider
            );
        }

        public override Dictionary<string, object?> MapToQueryParameters(BookedJob entity)
        {
            return new Dictionary<string, object?>
            {
                { "scheduled_date", entity.ScheduledDate.ToDateTime(TimeOnly.MinValue) },
                { "scheduled_time", entity.ScheduledTime?.ToTimeSpan() },
                { "duration_hours", entity.DurationHours },
                { "unit_price", entity.UnitPrice },
                { "is_substitution", entity.IsSubstitution },
                { "job_listing_id", entity.Listing.Id },
                { "neighborhood_id", entity.Neighborhood.Id },
                { "substituting_provider_id", entity.SubstitutingProvider?.Id }
            };
        }
    }
}