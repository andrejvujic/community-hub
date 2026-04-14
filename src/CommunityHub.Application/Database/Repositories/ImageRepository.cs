using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain;

namespace CommunityHub.Application.Database.Repositories
{
    public class ImageRepository
    {
        private readonly ImageMapper _imageMapper = new ImageMapper();
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
