using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain.NoticeBoard;
using System.Reflection.Metadata.Ecma335;
using static System.Net.Mime.MediaTypeNames;

namespace CommunityHub.Application.Database.Repositories
{
    public class AdRepository
    {
        private readonly AdMapper _adMapper = new();

        public List<Ad> GetByBuildingId(long id)
        {
            var query = PostgresQuery.Create(
                @"SELECT

                    ad.id AS ad_id,
                    ad.ad_category,
                    ad.ad_status,
                    ad.ad_type,
                    ad.building_id,
                    ad.description,
                    ad.start_date,
                    ad.end_date,

                    author.id AS author_user_id,
                    author.name AS author_name,
                    author.surname AS author_surname,
                    author.username AS author_username,
                    author.password AS author_password,
                    author.user_role AS author_user_role
                
                FROM ads ad 
                INNER JOIN users author ON ad.resident_id = author.id
                WHERE ad.building_id = @building_id"
            ).AddParameter("building_id", id);

            List<Ad> ads = new List<Ad>();
            query.ExecuteReader(
                reader => ads.Add(_adMapper.MapToEntity(reader))
            );

            return ads;
        }

        public List<Ad> GetByAuthorId(long id)
        {
            var query = PostgresQuery.Create(
                @"SELECT

                    ad.id AS ad_id,
                    ad.ad_category,
                    ad.ad_status,
                    ad.ad_type,
                    ad.building_id,
                    ad.description,
                    ad.start_date,
                    ad.end_date,

                    author.id AS author_user_id,
                    author.name AS author_name,
                    author.surname AS author_surname,
                    author.username AS author_username,
                    author.password AS author_password,
                    author.user_role AS author_user_role
                
                FROM ads ad 
                INNER JOIN users author ON ad.resident_id = author.id
                WHERE ad.resident_id = @author_id"
            ).AddParameter("author_id", id);

            List<Ad> ads = new List<Ad>();
            query.ExecuteReader(
                reader => ads.Add(_adMapper.MapToEntity(reader))
            );

            return ads;
        }

        public int GetAdsCountByDateAndType(long buildingId, AdType type, int year, int? month)
        {
            var query = PostgresQuery.Create(
                @"  SELECT COUNT(*)
                    FROM ads ad
                    WHERE ad.building_id = @building_id
                    AND ad.ad_type = @type
                    AND (EXTRACT(YEAR FROM ad.start_date) = @year)
                    AND (EXTRACT(MONTH FROM ad.start_date) = @month OR @month IS NULL);"
            ).AddParameter("building_id", buildingId)
             .AddParameter("type", type)
             .AddParameter("year", year)
             .AddParameter("month", month);

            return query.ExecuteScalar<int>(result => Convert.ToInt32(result));
        }

        public List<(AdCategory, int, int)> GetAdStatsByCategory(long buildingId, int year, int? month)
        {
            var query = PostgresQuery.Create(
                @"SELECT
                    ad.ad_category,
                    COUNT(*) FILTER (WHERE ad.ad_type = 'offer')   AS offer_count,
                    COUNT(*) FILTER (WHERE ad.ad_type = 'request') AS request_count
                FROM ads ad
                WHERE ad.building_id = @buildingId
                AND (EXTRACT(YEAR  FROM ad.start_date) = @year)
                AND (EXTRACT(MONTH FROM ad.start_date) = @month OR @month IS NULL)
                GROUP BY ad.ad_category
                ORDER BY ad.ad_category;");

            query.AddParameter("buildingId", buildingId);
            query.AddParameter("year", year);
            query.AddParameter("month", month);

            var result = new List<(AdCategory, int, int)>();

            query.ExecuteReader(reader =>
            {
                var category = Enum.Parse<AdCategory>(reader["ad_category"].ToString()!);
                var offerCount = Convert.ToInt32(reader["offer_count"]);
                var requestCount = Convert.ToInt32(reader["request_count"]);

                result.Add((category, offerCount, requestCount));
            });

            return result;
        }

        public List<(AdCategory, int)> GetActiveAdsByCategory(long buildingId)
        {
            var query = PostgresQuery.Create(
                @"SELECT
                    ad.ad_category,
                    COUNT(*) FILTER (WHERE ad.ad_status = 'active') AS active_ads
                FROM ads ad
                WHERE ad.building_id = @buildingId
                GROUP BY ad.ad_category
                ORDER BY ad.ad_category;");

            query.AddParameter("buildingId", buildingId);

            var result = new List<(AdCategory, int)>();

            query.ExecuteReader(reader =>
            {
                var category = Enum.Parse<AdCategory>(reader["ad_category"].ToString()!);
                var offerCount = Convert.ToInt32(reader["active_ads"]);

                result.Add((category, offerCount));
            });

            return result;
        }

        public int GetAdsCountByStatus(long buildingId, AdStatus status)
        {
            var query = PostgresQuery.Create(
                @"SELECT COUNT(*)
                    FROM ads ad
                    WHERE ad.building_id = @building_id
                    AND ad.ad_status = @status;"
            ).AddParameter("building_id", buildingId)
             .AddParameter("status", status);
            return query.ExecuteScalar<int>(result => Convert.ToInt32(result));
        }

        public string GetMostHelpfulResident(long buildingId)
        {
            var query = PostgresQuery.Create(
                @"SELECT u.name, u.surname, COUNT(*) AS zauzeti_termini
                    FROM ad_time_slots ts
                    INNER JOIN ads a   ON ts.ad_id = a.id
                    INNER JOIN users u ON a.resident_id = u.id
                    INNER JOIN ad_time_slot_reservations r ON r.offer_ad_time_slot_id = ts.id
                    WHERE a.building_id = @building_id
                    AND a.ad_type = 'offer'
                    GROUP BY u.id, u.username, u.surname
                    ORDER BY zauzeti_termini DESC
                    LIMIT 1;"
            )
            .AddParameter("building_id", buildingId);

            string? resident = null;

            query.ExecuteReader(reader =>
            {
                var username = reader["name"].ToString();
                var surname = reader["surname"].ToString();
                resident = $"{username} {surname}";
            });

            return resident ?? "N/A";
        }
    }
}
