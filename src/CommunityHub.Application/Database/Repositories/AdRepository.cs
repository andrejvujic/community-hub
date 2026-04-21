using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain.NoticeBoard;

namespace CommunityHub.Application.Database.Repositories
{
    public class AdRepository
    {
        private readonly AdMapper _adMapper = new();

        public List<Ad> GetByBuildingId(long buildingId)
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
            ).AddParameter("building_id", buildingId);

            List<Ad> ads = new List<Ad>();
            query.ExecuteReader(
                reader => ads.Add(_adMapper.MapToEntity(reader))
            );

            return ads;
        }  
    }
}
