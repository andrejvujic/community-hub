using CommunityHub.Application.Domain.RepositoryInterfaces.Addresses;
using CommunityHub.Application.Domain.RepositoryInterfaces.Neighborhoods;
using CommunityHub.Application.Helpers.Search;
using CommunityHub.Application.MappersDto.Addresses;
using CommunityHub.Application.MappersDto.Neighborhoods;
using CommunityHub.Application.Services.Addresses;
using CommunityHub.Application.Services.Dtos.Addresses;
using CommunityHub.Application.Services.Dtos.Neighborhoods;
using CommunityHub.Application.Services.Images;

namespace CommunityHub.Application.Services.Neighborhoods
{
    public class NeighborhoodService
    {
        private readonly INeighborhoodRepository _neighborhoodRepository
            = Injector.CreateInstance<INeighborhoodRepository>();
        private readonly IStreetSectionRepository _streetSectionRepository 
            = Injector.CreateInstance<IStreetSectionRepository>();
        private readonly NeighborhoodMapper _neighborhoodMapper = new();
        private readonly StreetSectionMapper _streetSectionMapper = new();
        private readonly AddressMapper _addressMapper = new();

        
        public List<NeighborhoodDto> GetByCoordinator(long coordinatorId)
            => _neighborhoodRepository.GetByCoordinatorId(coordinatorId).Select(_neighborhoodMapper.MapToDto).ToList();

        public NeighborhoodDto Create(NeighborhoodDto neighborhoodDto)
        {
            var neighborhood = _neighborhoodMapper.MapToDomain(neighborhoodDto);
            var created = _neighborhoodRepository.Create(neighborhood);

            return _neighborhoodMapper.MapToDto(created);
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
    }
}
