using CommunityHub.Application.Domain.Addresses;
using CommunityHub.Application.Domain.Addresses.RepositoryInterfaces;
using CommunityHub.Application.MappersDto;
using CommunityHub.Application.Services.Dtos.Addresses;

namespace CommunityHub.Application.Services.Addresses
{
    public class StreetSectionService
    {
        private readonly IStreetSectionRepository _streetSectionRepository 
            = Injector.CreateInstance<IStreetSectionRepository>();
        private readonly IMapper<StreetSection, StreetSectionDto> _streetSectionMapper
            = Injector.CreateInstance<IMapper<StreetSection, StreetSectionDto>>();

        public List<StreetSectionDto> GetByNeighborhood(long neighborhoodId) 
            => _streetSectionRepository.GetByNeighborhoodId(neighborhoodId)
                .Select(_streetSectionMapper.MapToDto)
                .ToList();
    }
}
