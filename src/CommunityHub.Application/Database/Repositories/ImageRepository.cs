using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain;

namespace CommunityHub.Application.Database.Repositories
{
    public class ImageRepository
    {
        private readonly ImageMapper _imageMapper = new ImageMapper();

        public Image CreateForBuilding(Image image, long buildingId)
        {
            var prepared = Image.ForBuilding(0, image.Path, buildingId);

            var query = PostgresQuery.Create(
                @"INSERT INTO images (path, building_id, neighborhood_id, job_listing_id)
                  VALUES (@path, @building_id, @neighborhood_id, @job_listing_id)
                  RETURNING id"
            )
            .ExtendParameters(_imageMapper.MapToQueryParameters(prepared));

            long generatedId = query.ExecuteScalar<long>(value => Convert.ToInt64(value));

            return Image.ForBuilding(generatedId, prepared.Path, buildingId);
        }

        public Image CreateForNeighborhood(Image image, long neighborhoodId)
        {
            var prepared = Image.ForNeighborhood(0, image.Path, neighborhoodId);

            var query = PostgresQuery.Create(
                @"INSERT INTO images (path, building_id, neighborhood_id, job_listing_id)
                  VALUES (@path, @building_id, @neighborhood_id, @job_listing_id)
                  RETURNING id"
            )
            .ExtendParameters(_imageMapper.MapToQueryParameters(prepared));

            long generatedId = query.ExecuteScalar<long>(value => Convert.ToInt64(value));

            return Image.ForNeighborhood(generatedId, prepared.Path, neighborhoodId);
        }

        public Image CreateForJobListing(Image image, long jobListingId)
        {
            var prepared = Image.ForJobListing(0, image.Path, jobListingId);

            var query = PostgresQuery.Create(
                @"INSERT INTO images (path, building_id, neighborhood_id, job_listing_id)
                  VALUES (@path, @building_id, @neighborhood_id, @job_listing_id)
                  RETURNING id"
            )
            .ExtendParameters(_imageMapper.MapToQueryParameters(prepared));

            long generatedId = query.ExecuteScalar<long>(value => Convert.ToInt64(value));

            return Image.ForJobListing(generatedId, prepared.Path, jobListingId);
        }

        public List<Image> GetByBuildingId(long buildingId)
        {
            var query = PostgresQuery.Create(
                @"SELECT
                    image.id AS image_id,
                    image.path AS path,
                    image.building_id AS image_building_id,
                    image.neighborhood_id AS image_neighborhood_id,
                    image.job_listing_id AS image_job_listing_id
                  FROM images image
                  WHERE image.building_id = @building_id"
            )
            .AddParameter("building_id", buildingId);

            var images = new List<Image>();
            query.ExecuteReader(reader =>
            {
                var image = _imageMapper.MapToEntity(reader);
                if (image is not null) images.Add(image);
            });

            return images;
        }

        public List<Image> GetByNeighborhoodId(long neighborhoodId)
        {
            var query = PostgresQuery.Create(
                @"SELECT
                    image.id AS image_id,
                    image.path AS path,
                    image.building_id AS image_building_id,
                    image.neighborhood_id AS image_neighborhood_id,
                    image.job_listing_id AS image_job_listing_id
                  FROM images image
                  WHERE image.neighborhood_id = @neighborhood_id"
            )
            .AddParameter("neighborhood_id", neighborhoodId);

            var images = new List<Image>();
            query.ExecuteReader(reader =>
            {
                var image = _imageMapper.MapToEntity(reader);
                if (image is not null) images.Add(image);
            });

            return images;
        }

        public List<Image> GetByJobListingId(long jobListingId)
        {
            var query = PostgresQuery.Create(
                @"SELECT
                    image.id AS image_id,
                    image.path AS path,
                    image.building_id AS image_building_id,
                    image.neighborhood_id AS image_neighborhood_id,
                    image.job_listing_id AS image_job_listing_id
                  FROM images image
                  WHERE image.job_listing_id = @job_listing_id"
            )
            .AddParameter("job_listing_id", jobListingId);

            var images = new List<Image>();
            query.ExecuteReader(reader =>
            {
                var image = _imageMapper.MapToEntity(reader);
                if (image is not null) images.Add(image);
            });

            return images;
        }

        public void DeleteByBuildingId(long buildingId)
        {
            PostgresQuery.Create(
                "DELETE FROM images WHERE building_id = @building_id"
            )
            .AddParameter("building_id", buildingId)
            .GetRowsAffectedCount();
        }

        public void DeleteByNeighborhoodId(long neighborhoodId)
        {
            PostgresQuery.Create(
                "DELETE FROM images WHERE neighborhood_id = @neighborhood_id"
            )
            .AddParameter("neighborhood_id", neighborhoodId)
            .GetRowsAffectedCount();
        }

        public void DeleteByJobListingId(long jobListingId)
        {
            PostgresQuery.Create(
                "DELETE FROM images WHERE job_listing_id = @job_listing_id"
            )
            .AddParameter("job_listing_id", jobListingId)
            .GetRowsAffectedCount();
        }

        // "Stari" kod:
        public Image Create(Image image, long? neighborhoodId = null, long? buildingId = null)
        {
            var preparedImage = image.CopyWith(neighborhoodId: neighborhoodId, buildingId: buildingId);

            var query = PostgresQuery.Create(
                @"INSERT INTO images (path, building_id, neighborhood_id) 
                VALUES (@path, @image_building_id, @image_neighborhood_id) 
                RETURNING id"
            )
            .ExtendParameters(_imageMapper.MapToQueryParameters(preparedImage));

            long generatedId = query.ExecuteScalar<long>(value => Convert.ToInt64(value));

            return preparedImage.CopyWith(id: generatedId);
        }
    }
}