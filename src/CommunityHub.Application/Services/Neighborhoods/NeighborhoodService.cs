using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.Addresses;
using CommunityHub.Application.Domain.Addresses.RepositoryInterfaces;
using CommunityHub.Application.Domain.Images;
using CommunityHub.Application.Domain.Neighborhoods;
using CommunityHub.Application.Domain.Neighborhoods.RepositoryInterfaces;
using CommunityHub.Application.Helpers.Search;
using CommunityHub.Application.MappersDto;
using CommunityHub.Application.Services.Dtos.Addresses;
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
        private readonly IMapper<Neighborhood, NeighborhoodDto> _neighborhoodMapper
            = Injector.CreateInstance<IMapper<Neighborhood, NeighborhoodDto>>();
        private readonly IMapper<StreetSection, StreetSectionDto> _streetSectionMapper
            = Injector.CreateInstance<IMapper<StreetSection, StreetSectionDto>>();
        private readonly IMapper<Address, AddressDto> _addressMapper
            = Injector.CreateInstance<IMapper<Address, AddressDto>>();

        public List<NeighborhoodDto> GetByCoordinator(long coordinatorId)
            => _neighborhoodRepository.GetByCoordinatorId(coordinatorId).Select(_neighborhoodMapper.MapToDto).ToList();

        public NeighborhoodDto Register(NeighborhoodDto neighborhoodDto)
        {
            var neighborhood = _neighborhoodMapper.MapToDomain(neighborhoodDto);
            var createdNeighborhood = _neighborhoodRepository.Create(neighborhood);
            RegisterStreetSections(neighborhoodDto.StreetSections, createdNeighborhood.Id);
            RegisterImages(neighborhood.Images.Select(i => i.Path).ToList(), createdNeighborhood.Id);

            return _neighborhoodMapper.MapToDto(createdNeighborhood);
        }

        public void RegisterStreetSections(List<StreetSectionDto> streetSectionsDto, long neighborhoodId) 
        {
            foreach (var sectionDto in streetSectionsDto)
                _streetSectionRepository.Create(_streetSectionMapper.MapToDomain(sectionDto), neighborhoodId);
        }

        public List<NeighborhoodDto> Search(NeighborhoodSearchParameters? searchParameters)
        {
            var neighborhoods = _neighborhoodRepository.Search(searchParameters ?? NeighborhoodSearchParameters.Empty());
            return neighborhoods.Select(_neighborhoodMapper.MapToDto).ToList();
        }

        public void LoadStreetSections(NeighborhoodDto neighborhoodDto)
        {
            if (neighborhoodDto.StreetSections.Any() || neighborhoodDto.Id == 0) 
                return;

            var loaded = _streetSectionRepository.GetByNeighborhoodId(neighborhoodDto.Id);
            neighborhoodDto.StreetSections.Clear();
            foreach (var s in loaded) 
                neighborhoodDto.StreetSections.Add(_streetSectionMapper.MapToDto(s));
        }

        public bool AddressIsInNeighborhood(NeighborhoodDto neighborhoodDto, AddressDto addressDto)
        {
            var neighborhood = _neighborhoodMapper.MapToDomain(neighborhoodDto);
            var address = _addressMapper.MapToDomain(addressDto);
            return neighborhood.Contains(address);
        }

        private void RegisterImages(List<string> imagePaths, long neighborhoodId)
        {
            foreach (var imagePath in imagePaths)
                _imageRepository.Create(Image.ForNeighborhood(0, imagePath, neighborhoodId));
        }
    }
}
