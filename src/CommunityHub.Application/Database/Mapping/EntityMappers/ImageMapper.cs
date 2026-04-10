using CommunityHub.Application.Domain;
using System.Data;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class ImageMapper : EntityMapper<Image>
    {
        public override Image? MapToEntity(IDataReader reader)
        {
            int idColumnIndex = reader.GetOrdinal("image_id");
            if (reader.IsDBNull(idColumnIndex))
                return null;

            long id = reader.GetInt64(idColumnIndex);

            string path = reader.GetString(reader.GetOrdinal("path"));

            int buildingIdColumnIndex = reader.GetOrdinal("image_building_id");
            long? buildingId = reader.IsDBNull(buildingIdColumnIndex) ? null : reader.GetInt64(buildingIdColumnIndex);

            int neighborhoodIdColumnIndex = reader.GetOrdinal("image_neighborhood_id");
            long? neighborhoodId = reader.IsDBNull(neighborhoodIdColumnIndex) ? null : reader.GetInt64(neighborhoodIdColumnIndex);

            return new Image(id, path, buildingId, neighborhoodId);
        }

        public override Dictionary<string, object?> MapToQueryParameters(Image entity)
        {
            return new Dictionary<string, object?>
            {
                { "image_id", entity.Id },
                { "path", entity.Path  },
                { "image_building_id", entity.BuildingId },
                { "image_neighborhood_id", entity.NeighborhoodId }
            };
        }
    }
}
