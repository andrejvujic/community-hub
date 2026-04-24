using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain.NoticeBoard;
using System.Reflection.Metadata.Ecma335;
using static System.Net.Mime.MediaTypeNames;

namespace CommunityHub.Application.Database.Repositories
{
    public class AdRepository
    {
        private readonly AdMapper _adMapper = new();

        public Ad Create(Ad ad)
        {
            var query = PostgresQuery.Create(
                @"INSERT INTO ads 
                    (building_id,
                    resident_id,
                    ad_type,
                    ad_category,
                    ad_status,
                    description,
                    start_date,
                    end_date)
                VALUES 
                    (@building_id,
                    @author_id,
                    @ad_type,
                    @ad_category,
                    @ad_status,
                    @description,
                    @start_date,
                    @end_date)
                RETURNING id"
            ).ExtendParameters(_adMapper.MapToQueryParameters(ad));

            long id = query.ExecuteScalar<long>(value => Convert.ToInt64(value));;

            return ad.CopyWith(id: id);
        }

        public void Update(Ad ad)
        {
            PostgresQuery.Create(
                @"UPDATE ads SET
                    building_id = @building_id,
                    resident_id = @author_id,
                    ad_type = @ad_type,
                    ad_category = @ad_category,
                    ad_status = @ad_status,
                    description = @description,
                    start_date = @start_date,
                    end_date = @end_date
                WHERE id = @id"
            )
            .AddParameter("id", ad.Id)
            .ExtendParameters(_adMapper.MapToQueryParameters(ad))
            .Execute();
        }

        public List<Ad> GetForBuilding(long buildingId)
        {
            var query = PostgresQuery.Create(
                @"SELECT

                    ad.id AS ad_id,
                    ad.ad_category,
                    ad.ad_status,
                    ad.ad_type,
                    ad.building_id AS ad_building_id,
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
                WHERE ad.building_id = @building_id
                ORDER BY ad_id ASC"
            ).AddParameter("building_id", buildingId);

            List<Ad> ads = new List<Ad>();
            query.ExecuteReader(
                reader => ads.Add(_adMapper.MapToEntity(reader))
            );

            return ads;
        }

        public List<Ad> GetAuthorAdsForBuilding(long authorId, long buildingId)
        {
            var query = PostgresQuery.Create(
                @"SELECT

                    ad.id AS ad_id,
                    ad.ad_category,
                    ad.ad_status,
                    ad.ad_type,
                    ad.building_id AS ad_building_id,
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
                WHERE ad.resident_id = @author_id AND ad.building_id = @building_id
                ORDER BY ad_id ASC"
            ).AddParameter("author_id", authorId)
            .AddParameter("building_id", buildingId);

            List<Ad> ads = new List<Ad>();
            query.ExecuteReader(
                reader => ads.Add(_adMapper.MapToEntity(reader))
            );

            return ads;
        }

        public List<Ad> GetMatchingAds(long adId)
        {
            var query = PostgresQuery.Create(
                @"SELECT 

	                ad_2.id AS ad_id,
	                ad_2.building_id AS ad_building_id,
	                ad_2.ad_type,
	                ad_2.ad_category,
	                ad_2.ad_status,
	                ad_2.description,
	                ad_2.start_date,
	                ad_2.end_date,

	                author_2.id AS author_user_id,
	                author_2.name AS author_name,
	                author_2.surname AS author_surname,
	                author_2.username AS author_username,
	                author_2.password AS author_password,
	                author_2.user_role AS author_user_role

                FROM ads ad_1 
                INNER JOIN ads ad_2 ON

	                (DATERANGE(ad_2.start_date, ad_2.end_date, '[]') && DATERANGE(ad_1.start_date, ad_1.end_date, '[]')) AND
	                ad_2.building_id = ad_1.building_id AND
	                ad_2.resident_id <> ad_1.resident_id AND
	                ad_2.ad_category = ad_1.ad_category AND
	                ad_2.ad_type <> ad_1.ad_type AND 
	                ad_2.ad_status = 'active' AND
                    ad_2.resident_id <> ad_1.resident_id

                INNER JOIN users author_2 ON author_2.id = ad_2.resident_id
                WHERE ad_1.id = @ad_id"
            ).AddParameter("ad_id", adId);

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
                @"SELECT u.name, u.surname, COUNT(*) AS busy_time_slots
                    FROM ad_time_slots ts
                    INNER JOIN ads a   ON ts.ad_id = a.id
                    INNER JOIN users u ON a.resident_id = u.id
                    INNER JOIN ad_time_slot_reservations r ON r.offer_ad_time_slot_id = ts.id
                    WHERE a.building_id = @building_id
                    AND a.ad_type = 'offer'
                    GROUP BY u.id, u.username, u.surname
                    ORDER BY busy_time_slots DESC
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
