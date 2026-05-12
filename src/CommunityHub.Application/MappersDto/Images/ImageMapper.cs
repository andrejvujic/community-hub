using CommunityHub.Application.Domain;
using CommunityHub.Application.Services.Dtos.Images;

namespace CommunityHub.Application.MappersDto.Images
{
    public class ImageMapper : IMapper<Image, ImageDto>
    {
        public ImageDto MapToDto(Image image)
        {
            if (image == null) throw new ArgumentNullException(nameof(image));
            return new ImageDto(
                image.Id,
                image.Path,
                image.BuildingId,
                image.NeighborhoodId,
                image.JobListingId
            );
        }

        public Image MapToDomain(ImageDto imageDto)
        {
            if (imageDto == null) throw new ArgumentNullException(nameof(imageDto));

            if (imageDto.BuildingId.HasValue)
                return Image.ForBuilding(imageDto.Id, imageDto.Path, imageDto.BuildingId.Value);

            if (imageDto.NeighborhoodId.HasValue)
                return Image.ForNeighborhood(imageDto.Id, imageDto.Path, imageDto.NeighborhoodId.Value);

            if (imageDto.JobListingId.HasValue)
                return Image.ForJobListing(imageDto.Id, imageDto.Path, imageDto.JobListingId.Value);

            throw new Exception($"Couldn't read image '{imageDto.Id}' because building_id = neighborhood_id = job_listing_id = NULL");
        }
    }
}

