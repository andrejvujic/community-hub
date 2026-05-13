using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.Addresses;
using CommunityHub.Application.Domain.Addresses.RepositoryInterfaces;
using CommunityHub.Application.Domain.Images;
using CommunityHub.Application.Domain.Neighborhoods;
using CommunityHub.Application.Domain.Neighborhoods.RepositoryInterfaces;
using CommunityHub.Application.Services.Dtos.Neighborhoods;

namespace CommunityHub.Application.Services.Neighborhoods
{
    public class NeighborhoodService
    {
        private readonly INeighborhoodRepository _neighborhoodRepository
            = Injector.CreateInstance<INeighborhoodRepository>();
        private readonly IStreetSectionRepository _streetSectionRepository 
            = Injector.CreateInstance<IStreetSectionRepository>();
        private readonly IImageRepository _imageRepository 
            = Injector.CreateInstance<IImageRepository>();

        public List<Neighborhood> GetByCoordinator(long coordinatorId)
            => _neighborhoodRepository.GetByCoordinatorId(coordinatorId);

        public Neighborhood Register(Neighborhood neighborhood)
        {
            Neighborhood createdNeighborhood = _neighborhoodRepository.Create(neighborhood);
            RegisterStreetSections(neighborhood.StreetSections, createdNeighborhood.Id);
            
            return createdNeighborhood;
        }

        public void RegisterStreetSections(List<StreetSection> streetSections, long neighborhoodId) 
        {
            foreach (var section in streetSections)
                _streetSectionRepository.Create(section, neighborhoodId);
        }

        private void RegisterImages(List<string> imagePaths, long neighborhoodId)
        {
            foreach (var imagePath in imagePaths)
                _imageRepository.Create(Image.ForNeighborhood(0, imagePath, neighborhoodId));
        }
    }
}
