using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.RepositoryInterfaces.Images;

namespace CommunityHub.Application.Database.Repositories.Shared
{
    public class ImageRepository : IImageRepository
    {
        private readonly ImageMapper _imageMapper = new();

        public Image Create(Image image)
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery.Create(
                @"INSERT INTO images (path, building_id, neighborhood_id, job_listing_id) 
                VALUES (@path, @image_building_id, @image_neighborhood_id, @image_job_listing_id) 
                RETURNING id"
            )
            .ExtendParameters(_imageMapper.MapToQueryParameters(image));

            long id = connection.ExecuteScalar(query, Convert.ToInt64);

            return image.CopyWith(id: id);
        }

        public List<Image> GetByBuilding(long buildingId)
        {
            var query = PostgresQuery.Create(
                @"SELECT
                    image.id AS image_id,
                    image.path AS path,
                    image.building_id AS image_building_id
                  FROM images image
                  WHERE image.building_id = @building_id"
            )
            .AddParameter("building_id", buildingId);

            var images = new List<Image>();

            query.ExecuteReader(
                reader => {
                    var image = _imageMapper.MapToEntity(reader);
                    if (image is not null)
                        images.Add(image);
                }
            );

            return images;
        }

        public List<Image> GetByNeighborhoodId(long neighborhoodId)
        {
            var query = PostgresQuery.Create(
                @"SELECT
                    image.id AS image_id,
                    image.path AS path,
                    image.neighborhood_id AS image_neighborhood_id
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
    }
}