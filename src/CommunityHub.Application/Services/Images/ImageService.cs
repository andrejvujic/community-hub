using CommunityHub.Application.Domain.RepositoryInterfaces.Images;
using CommunityHub.Application.MappersDto.Images;
using CommunityHub.Application.Services.Dtos.Images;
using DomainImage = CommunityHub.Application.Domain.Image;

namespace CommunityHub.Application.Services.Images
{
    public class ImageService
    {
        private readonly IImageRepository _imageRepository = Injector.CreateInstance<IImageRepository>();
        private readonly ImageMapper _imageMapper = new();

        public DomainImage Create(DomainImage image)
            => _imageRepository.Create(image);

        public List<ImageDto> GetByNeighborhood(long neighborhoodId)
            => _imageRepository
                .GetByNeighborhoodId(neighborhoodId)
                .Select(_imageMapper.MapToDto)
                .ToList();

        public ImageDto CreateForNeighborhood(string path, long neighborhoodId)
        {
            var image = DomainImage.ForNeighborhood(0, path, neighborhoodId);
            var created = _imageRepository.Create(image);
            return _imageMapper.MapToDto(created);
        }
    }
}