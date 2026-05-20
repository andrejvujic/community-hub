using CommunityHub.Application.Domain.Addresses;
using CommunityHub.Application.Services.Dtos.Addresses;
using CommunityHub.Application.MappersDto.Addresses;
using CommunityHub.Application.Domain.RepositoryInterfaces.Addresses;

namespace CommunityHub.Application.Services.Addresses
{
    public class StreetService
    {
        private readonly IStreetRepository _streetRepository = Injector.CreateInstance<IStreetRepository>();
        private readonly StreetMapper _streetMapper = new();
        public List<StreetDto> GetByCityId(long cityId)
        {
            List<Street> streets = _streetRepository.GetByCityId(cityId);
            List<StreetDto> streetsDto = [];

            foreach (Street street in streets)
                streetsDto.Add(_streetMapper.MapToDto(street));

            return streetsDto;
        }
    }
}