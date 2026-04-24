using System.Collections;
using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain;

namespace CommunityHub.Application.Database.Repositories
{
    public class BookedJobRepository
    {
        private readonly BookedJobMapper _bookedJobMapper = new BookedJobMapper();

        public List<BookedJob> GetByNeighborhoodId(long neighborhoodId)
        {
            var query = PostgresQuery.Create(
                @"SELECT
                    booked_job.id AS booked_job_id,
                    booked_job.scheduled_date,
                    booked_job.scheduled_time,
                    booked_job.duration_hours,
                    booked_job.unit_price,
                    booked_job.is_substitution,

                    job_listing.id AS job_listing_id,
                    job_listing.service_name,
                    job_listing.description,
                    job_listing.billing_type,

                    listing_city.id AS listing_city_city_id,
                    listing_city.name AS listing_city_city_name,

                    listing_country.id AS listing_country_country_id,
                    listing_country.name AS listing_country_country_name,

                    users.id AS user_id,
                    users.username,
                    users.password,
                    users.name,
                    users.surname,
                    users.user_role,

                    provider_city.id AS provider_city_city_id,
                    provider_city.name AS provider_city_city_name,

                    provider_country.id AS provider_country_country_id,
                    provider_country.name AS provider_country_country_name,

                    neighborhood.id AS neighborhood_id,
                    neighborhood.name AS neighborhood_name,
                    neighborhood.description,
                    neighborhood.budget,

                    city.id AS city_id,
                    city.name AS city_name,

                    country.id AS country_id,
                    country.name AS country_name,

                    coordinator.id AS coordinator_user_id,
                    coordinator.username AS coordinator_username,
                    coordinator.password AS coordinator_password,
                    coordinator.name AS coordinator_name,
                    coordinator.surname AS coordinator_surname,
                    coordinator.user_role AS coordinator_user_role

                FROM booked_jobs booked_job
                INNER JOIN job_listings job_listing ON job_listing.id = booked_job.job_listing_id
                INNER JOIN service_providers service_provider ON service_provider.id = job_listing.provider_id
                INNER JOIN users ON users.id = service_provider.id
                INNER JOIN cities listing_city ON listing_city.id = job_listing.city_id
                INNER JOIN countries listing_country ON listing_country.id = job_listing.country_id
                INNER JOIN cities provider_city ON provider_city.id = service_provider.city_id
                INNER JOIN countries provider_country ON provider_country.id = service_provider.country_id
                INNER JOIN neighborhoods neighborhood ON neighborhood.id = booked_job.neighborhood_id
                INNER JOIN cities city ON city.id = neighborhood.city_id
                INNER JOIN countries country ON country.id = city.country_id
                INNER JOIN users coordinator ON coordinator.id = neighborhood.coordinator_id
                WHERE booked_job.neighborhood_id = @neighborhood_id
                ORDER BY booked_job.scheduled_date, booked_job.scheduled_time"
            ).AddParameter("neighborhood_id", neighborhoodId);

            List<BookedJob> bookedJobs = new List<BookedJob>();
            query.ExecuteReader(reader =>{
                bookedJobs.Add(_bookedJobMapper.MapToEntity(reader));
            });
            return bookedJobs;
        }

        public BookedJob Create(BookedJob bookedJob)
        {
            var query = PostgresQuery.Create(
                @"INSERT INTO booked_jobs
                    (scheduled_date, scheduled_time, duration_hours, unit_price, is_substitution, job_listing_id, neighborhood_id, substituting_provider_id)
                  VALUES
                    (@scheduled_date, @scheduled_time, @duration_hours, @unit_price, @is_substitution, @job_listing_id, @neighborhood_id, @substituting_provider_id)
                  RETURNING id"
            )
            .ExtendParameters(_bookedJobMapper.MapToQueryParameters(bookedJob));
            
            long bookedJobId = query.ExecuteScalar<long>(value => Convert.ToInt64(value));

            return bookedJob.CopyWith(id: bookedJobId);
        }

        public List<(DateOnly Date, TimeOnly? Time, int? DurationHours)> GetOccupiedSlots(long jobListingId)
        {
            var query = PostgresQuery.Create(
                @"SELECT
                    booked_job.scheduled_date,
                    booked_job.scheduled_time,
                    booked_job.duration_hours
                FROM booked_jobs booked_job
                WHERE booked_job.job_listing_id = @job_listing_id
                ORDER BY booked_job.scheduled_date, booked_job.scheduled_time"
            ).AddParameter("job_listing_id", jobListingId);

            var results = new List<(DateOnly, TimeOnly?, int?)>();
            query.ExecuteReader(reader =>
            {
                var date = DateOnly.FromDateTime(
                    reader.GetDateTime(reader.GetOrdinal("scheduled_date")));

                int timeOrdinal = reader.GetOrdinal("scheduled_time");
                TimeOnly? time = reader.IsDBNull(timeOrdinal) 
                    ? null
                    : (TimeOnly)reader.GetValue(timeOrdinal);

                int durationOrdinal = reader.GetOrdinal("duration_hours");
                int? duration = reader.IsDBNull(durationOrdinal)
                    ? null
                    : reader.GetInt32(durationOrdinal);

                results.Add((date, time, duration));
            });

            return results;
        }
    }
}
