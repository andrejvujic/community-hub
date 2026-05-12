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
        public Image MapToDomain(ImageDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            if (dto.BuildingId.HasValue)
                return Image.ForBuilding(dto.Id, dto.Path, dto.BuildingId.Value);

            if (dto.NeighborhoodId.HasValue)
                return Image.ForNeighborhood(dto.Id, dto.Path, dto.NeighborhoodId.Value);

            if (dto.JobListingId.HasValue)
                return Image.ForJobListing(dto.Id, dto.Path, dto.JobListingId.Value);

            throw new Exception($"Couldn't read image '{dto.Id}' because building_id = neighborhood_id = job_listing_id = NULL");
        }
    }
}