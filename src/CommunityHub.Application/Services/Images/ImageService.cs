using CommunityHub.Application.Domain.Images;
using DomainImage = CommunityHub.Application.Domain.Image;

namespace CommunityHub.Application.Services.Images
{
    public class ImageService
    {
        private readonly IImageRepository _imageRepository = Injector.CreateInstance<IImageRepository>();

        public DomainImage Create(DomainImage image)
            => _imageRepository.Create(image);
    }
}