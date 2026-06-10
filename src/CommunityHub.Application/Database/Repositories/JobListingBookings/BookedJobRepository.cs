using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain.JobListingBookings;
using CommunityHub.Application.Domain.RepositoryInterfaces.JobListingBookings;

namespace CommunityHub.Application.Database.Repositories.JobListingBookings
{
    public class BookedJobRepository : IBookedJobRepository
    {
        private readonly BookedJobMapper _bookedJobMapper = new BookedJobMapper();

        private const string SelectColumns = @"
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
            coordinator.user_role AS coordinator_user_role,
 
            sub_u.id AS sub_provider_user_id,
            sub_u.username AS sub_provider_username,
            sub_u.password AS sub_provider_password,
            sub_u.name AS sub_provider_name,
            sub_u.surname AS sub_provider_surname,
            sub_u.user_role AS sub_provider_user_role,
            sub_c.id AS sub_provider_city_id,
            sub_c.name AS sub_provider_city_name,
            sub_co.id AS sub_provider_country_id,
            sub_co.name AS sub_provider_country_name,
 
            pr.id AS review_id,
            pr.quality_rating,
            pr.communication_rating,
            pr.comment AS review_comment,
            pr.created_at AS review_created_at,
            pr.updated_at,
            review_coordinator.id AS review_coordinator_user_id,
            review_coordinator.username AS review_coordinator_username,
            review_coordinator.password AS review_coordinator_password,
            review_coordinator.name AS review_coordinator_name,
            review_coordinator.surname AS review_coordinator_surname,
            review_coordinator.user_role AS review_coordinator_user_role";

        private const string FromJoins = @"
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
            LEFT JOIN service_providers sub_sp ON sub_sp.id = booked_job.substituting_provider_id
            LEFT JOIN users sub_u ON sub_u.id = sub_sp.id
            LEFT JOIN cities sub_c ON sub_c.id = sub_sp.city_id
            LEFT JOIN countries sub_co ON sub_co.id = sub_sp.country_id
            LEFT JOIN provider_reviews pr ON pr.booked_job_id = booked_job.id
            LEFT JOIN users review_coordinator ON review_coordinator.id = pr.coordinator_id";

        public List<BookedJob> GetByNeighborhoodId(long neighborhoodId)
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery.Create(
                $@"SELECT {SelectColumns} {FromJoins}
                   WHERE booked_job.neighborhood_id = @neighborhood_id
                   ORDER BY booked_job.scheduled_date, booked_job.scheduled_time"
            ).AddParameter("neighborhood_id", neighborhoodId);

            List<BookedJob> bookedJobs = new List<BookedJob>();
            connection.ExecuteReader(query, reader => {
                var bookedJob = _bookedJobMapper.MapToEntity(reader);
                if (bookedJob is not null) bookedJobs.Add(bookedJob);
            });
            return bookedJobs;
        }

        public BookedJob Create(BookedJob bookedJob)
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery.Create(
                @"INSERT INTO booked_jobs
                    (scheduled_date, scheduled_time, duration_hours, unit_price, is_substitution, job_listing_id, neighborhood_id, substituting_provider_id)
                  VALUES
                    (@scheduled_date, @scheduled_time, @duration_hours, @unit_price, @is_substitution, @job_listing_id, @neighborhood_id, @substituting_provider_id)
                  RETURNING id"
            )
            .ExtendParameters(_bookedJobMapper.MapToQueryParameters(bookedJob));
            
            long bookedJobId = connection.ExecuteScalar(query, Convert.ToInt64);

            return bookedJob.CopyWith(id: bookedJobId);
        }

        public List<BookedJob> GetByServiceProviderId(long serviceProviderId)
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery.Create(
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
                WHERE users.id = @service_provider_id
                ORDER BY booked_job.scheduled_date, booked_job.scheduled_time"
            ).AddParameter("service_provider_id", serviceProviderId);

            List<BookedJob> bookedJobs = new List<BookedJob>();
            connection.ExecuteReader(query, reader => {
                var bookedJob = _bookedJobMapper.MapToEntity(reader);
                if (bookedJob is not null) bookedJobs.Add(bookedJob);
            });
            return bookedJobs;
        }

        public List<(DateOnly Date, TimeOnly? Time, int? DurationHours)> GetOccupiedSlots(long jobListingId)
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery.Create(
                @"SELECT
                    booked_job.scheduled_date,
                    booked_job.scheduled_time,
                    booked_job.duration_hours
                FROM booked_jobs booked_job
                WHERE booked_job.job_listing_id = @job_listing_id
                ORDER BY booked_job.scheduled_date, booked_job.scheduled_time"
            ).AddParameter("job_listing_id", jobListingId);

            var results = new List<(DateOnly, TimeOnly?, int?)>();
            connection.ExecuteReader(query, reader =>
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

        public List<BookedJob> GetAllFinishedByCoordinatorId(long coordinatorId)
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery.Create(
                $@"SELECT {SelectColumns} {FromJoins}
                   WHERE coordinator.id = @coordinator_id
                     AND booked_job.scheduled_date <= CURRENT_DATE
                   ORDER BY booked_job.scheduled_date DESC, booked_job.scheduled_time DESC"
            ).AddParameter("coordinator_id", coordinatorId);

            var results = new List<BookedJob>();
            connection.ExecuteReader(query, reader =>
            {
                var bookedJob = _bookedJobMapper.MapToEntity(reader);
                if (bookedJob is not null) results.Add(bookedJob);
            });
            return results;
        }
    }
}
