using CommunityHub.Application.Domain.RepositoryInterfaces.Addresses;
using CommunityHub.Application.MappersDto.Addresses;
using CommunityHub.Application.Services.Dtos.Addresses;

namespace CommunityHub.Application.Services.Addresses
{
    public class StreetSectionService
    {
        private readonly IStreetSectionRepository _streetSectionRepository = Injector.CreateInstance<IStreetSectionRepository>();
        private readonly StreetSectionMapper _streetSectionMapper= new StreetSectionMapper();

        public int GetStartNumber(long? streetId, long? cityId)
            => _streetSectionRepository.GetStartNumber(streetId, cityId);

        public int GetEndNumber(long? streetId, long? cityId)
            => _streetSectionRepository.GetEndNumber(streetId, cityId);

        public List<StreetSectionDto> GetByNeighborhood(long neighborhoodId)
            => _streetSectionRepository
                .GetByNeighborhoodId(neighborhoodId)
                .Select(_streetSectionMapper.MapToDto)
                .ToList();

        public StreetSectionDto Create(StreetSectionDto dto, long neighborhoodId)
        {
            var created = _streetSectionRepository.Create(_streetSectionMapper.MapToDomain(dto), neighborhoodId);
            return _streetSectionMapper.MapToDto(created);
        }
    }
}