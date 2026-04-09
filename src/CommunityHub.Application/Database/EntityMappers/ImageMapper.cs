using CommunityHub.Application.Domain;
using System.Data;

namespace CommunityHub.Application.Database.EntityMappers
{
    public class ImageMapper : EntityMapper<Image>
    {
        public override Image? MapToEntity(IDataReader reader, string? columnNamePrefix = null)
        {
            int idColumnIndex = reader.GetOrdinalPrefixed("image_id", columnNamePrefix);
            if (reader.IsDBNull(idColumnIndex))
                return null;

            long id = reader.GetInt64(idColumnIndex);

            string path = reader.GetString(reader.GetOrdinalPrefixed("path", columnNamePrefix));

            int buildingIdColumnIndex = reader.GetOrdinalPrefixed("image_building_id", columnNamePrefix);
            long? buildingId = reader.IsDBNull(buildingIdColumnIndex) ? null : reader.GetInt64(buildingIdColumnIndex);

            int neighborhoodIdColumnIndex = reader.GetOrdinalPrefixed("image_neighborhood_id", columnNamePrefix);
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
