using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads;
using System.Data;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class AdMapper : EntityMapper<Ad>
    {
        private readonly UserMapper _userMapper = new();

        public override Ad? MapToEntity(IDataReader reader)
        {
            if (!reader.HasColumn("ad_id")) return null;

            var idColumnIndex = reader.GetOrdinal("ad_id");
            if (reader.IsDBNull(idColumnIndex)) return null;

            var id = reader.GetInt64(idColumnIndex);

            var author = _userMapper.MapToEntity(reader.WithPrefix("author"));
            if (author == null)
                throw new Exception("Couldn't convert database row to entity because of missing data. Author can't be null.");


            var category = (AdCategory)reader.GetValue(reader.GetOrdinal("ad_category"));
            var status = (AdStatus)reader.GetValue(reader.GetOrdinal("ad_status"));
            var type = (AdType)reader.GetValue(reader.GetOrdinal("ad_type"));

            var buildingId = reader.GetInt64(reader.GetOrdinal("ad_building_id"));

            var description = reader.GetString(reader.GetOrdinal("description"));

            var startDate = reader.GetDateTime(reader.GetOrdinal("start_date"));
            var endDate = reader.GetDateTime(reader.GetOrdinal("end_date"));

            return new Ad(
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
