using CommunityHub.Application.Domain.Addresses.RepositoryInterfaces;
using CommunityHub.Application.Domain.Images;
using CommunityHub.Application.Domain.Neighborhoods;
using CommunityHub.Application.Domain.Neighborhoods.RepositoryInterfaces;
using CommunityHub.Application.MappersDto;
using CommunityHub.Application.Services.Dtos.Neighborhoods;

namespace CommunityHub.Application.Services.Neighborhoods
{
    public class NeighborhoodsService
    {
        private readonly INeighborhoodRepository _neighborhoodRepository
            = Injector.CreateInstance<INeighborhoodRepository>();
        private readonly IStreetSectionRepository _streetSectionRepository 
            = Injector.CreateInstance<IStreetSectionRepository>();
        private readonly IImageRepository _imageRepository 
            = Injector.CreateInstance<IImageRepository>();
    }
}
