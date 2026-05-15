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

            if (reader.HasColumn("image_building_id"))
            {
                int columnIndex = reader.GetOrdinal("image_building_id");
                long? buildingId = reader.IsDBNull(columnIndex) ? null : reader.GetInt64(columnIndex);

                if (buildingId != null) return Image.ForBuilding(
                    id,
                    path,
                    buildingId: buildingId.Value
                );
            }

            if (reader.HasColumn("image_neighborhood_id"))
            {
                int columnIndex = reader.GetOrdinal("image_neighborhood_id");
                long? neighborhoodId = reader.IsDBNull(columnIndex) ? null : reader.GetInt64(columnIndex);

                if (neighborhoodId != null) return Image.ForNeighborhood(
                    id,
                    path,
                    neighborhoodId: neighborhoodId.Value
                );
            }

            if (reader.HasColumn("image_job_listing_id"))
            {
                int columnIndex = reader.GetOrdinal("image_job_listing_id");
                long? jobListingId = reader.IsDBNull(columnIndex) ? null : reader.GetInt64(columnIndex);

                if (jobListingId != null) return Image.ForJobListing(
                    id,
                    path,
                    jobListingId: jobListingId.Value
                );
            }

            throw new Exception($"Couldn't read image '{id}' because building_id = neighborhood_id = image_job_listing_id = NULL");
        }

        public override Dictionary<string, object?> MapToQueryParameters(Image entity)
        {
            return new Dictionary<string, object?>
            {
                { "path", entity.Path },
                { "image_building_id", entity.BuildingId },
                { "image_neighborhood_id", entity.NeighborhoodId },
                { "image_job_listing_id", entity.JobListingId }
            };
        }
    }
}