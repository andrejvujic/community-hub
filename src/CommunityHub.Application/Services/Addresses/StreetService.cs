using CommunityHub.Application.Domain.Addresses.RepositoryInterfaces;
using CommunityHub.Application.Domain.Addresses;
using CommunityHub.Application.Services.Dtos.Addresses;
using CommunityHub.Application.MappersDto.Addresses;

namespace CommunityHub.Application.Services.Addresses
{
    public class StreetService
    {
        private readonly IStreetRepository _streetRepository = Injector.CreateInstance<IStreetRepository>();
        private readonly StreetMapper _streetMapper = new();

        public List<Street> GetByCityId(long cityId)
            => _streetRepository.GetByCityId(cityId);

        public List<StreetDto> GetByCity(long cityId)
            => _streetRepository.GetByCityId(cityId).Select(_streetMapper.MapToDto).ToList();
    }
}