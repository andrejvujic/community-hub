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

            int jobListingIdColumnIndex = reader.GetOrdinal("image_job_listing_id");
            long? jobListingId = reader.IsDBNull(jobListingIdColumnIndex) ? null : reader.GetInt64(jobListingIdColumnIndex);

            if (buildingId != null) return Image.ForBuilding(id, path, buildingId.Value);
            if (neighborhoodId != null) return Image.ForNeighborhood(id, path, neighborhoodId.Value);
            if (jobListingId != null) return Image.ForJobListing(id, path, jobListingId.Value);

            throw new InvalidOperationException(
                $"Slika sa id={id} nema nijednog vlasnika (building_id, neighborhood_id i job_listing_id su svi NULL). " +
                "Ovo narušava single_owner constraint i ukazuje na korumpiran zapis u bazi.");
        }

        public override Dictionary<string, object?> MapToQueryParameters(Image entity)
        {
            return new Dictionary<string, object?>
            {
                { "path", entity.Path },
                { "building_id", entity.BuildingId },
                { "neighborhood_id", entity.NeighborhoodId },
                { "job_listing_id", entity.JobListingId }
            };
        }
    }
}