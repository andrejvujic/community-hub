using CommunityHub.Application.Domain.Ads;
using System.Data;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class AdMapper : EntityMapper<Ad>
    {
        private readonly UserMapper _userMapper = new();

        public override Ad? MapToEntity(IDataReader reader)
        {
            if (!reader.HasColumn("ad_id")) return null;

            var idColumnOrdinal = reader.GetOrdinal("ad_id");
            if (reader.IsDBNull(idColumnOrdinal)) return null;

            var id = reader.GetInt64(idColumnOrdinal);

            var authorId = reader.GetInt64(reader.GetOrdinal("author_id"));
            var author = _userMapper.MapToEntity(reader.WithPrefix("author"));

            var category = (AdCategory)reader.GetValue(reader.GetOrdinal("ad_category"));
            var status = (AdStatus)reader.GetValue(reader.GetOrdinal("ad_status"));
            var type = (AdType)reader.GetValue(reader.GetOrdinal("ad_type"));

            var buildingId = reader.GetInt64(reader.GetOrdinal("ad_building_id"));

            var description = reader.GetString(reader.GetOrdinal("description"));

            var startDate = reader.GetDateTime(reader.GetOrdinal("start_date"));
            var endDate = reader.GetDateTime(reader.GetOrdinal("end_date"));

            return new Ad(
                authorId: authorId,
                author: author,
                buildingId: buildingId,
                type: type,
                category: category,
                status: status,
                description: description,
                startDate: startDate,
                endDate: endDate,
                id: id
            );
        }

        public override Dictionary<string, object?> MapToQueryParameters(Ad entity)
        {
            return new Dictionary<string, object?>
            {
                { "author_id", entity.AuthorId },
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
