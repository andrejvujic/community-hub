using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.Buildings.Ads;
using System.Data;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class AdMapper : EntityMapper<Ad>
    {
        private readonly UserMapper _userMapper = new();

        public override Ad? MapToEntity(IDataReader reader)
        {
            if (!reader.HasColumn("ad_id")) return null;

            int idColumnIndex = reader.GetOrdinal("ad_id");
            if (reader.IsDBNull(idColumnIndex)) return null;

            long id = reader.GetInt64(idColumnIndex);

            User? author = _userMapper.MapToEntity(reader.WithPrefix("author"));
            if (author == null)
                throw new Exception("Couldn't convert database row to entity because of missing data. Author can't be null.");


            AdCategory category = (AdCategory)reader.GetValue(reader.GetOrdinal("ad_category"));
            AdStatus status = (AdStatus)reader.GetValue(reader.GetOrdinal("ad_status"));
            AdType type = (AdType)reader.GetValue(reader.GetOrdinal("ad_type"));

            long buildingId = reader.GetInt64(reader.GetOrdinal("ad_building_id"));

            string description = reader.GetString(reader.GetOrdinal("description"));

            DateTime startDate = reader.GetDateTime(reader.GetOrdinal("start_date"));
            DateTime endDate = reader.GetDateTime(reader.GetOrdinal("end_date"));

            return new Ad(
                author,
                buildingId,
                type,
                category,
                status,
                description,
                startDate,
                endDate,
                id
            );
        }

        public override Dictionary<string, object?> MapToQueryParameters(Ad entity)
        {
            return new Dictionary<string, object?>
            {
                { "author_id", entity.Author.Id },
                { "building_id", entity.BuildingId },
                { "description", entity.Description },
                { "ad_category", entity.Category },
                { "ad_status", entity.Status },
                { "ad_type", entity.Type },
                { "start_date", entity.StartDate },
                { "end_date", entity.EndDate }
            };
        }
    }
}
