using CommunityHub.Application.Domain.Apartments;
using CommunityHub.Application.Domain.RepositoryInterfaces.Apartments;
using CommunityHub.Application.MappersDto.Apartments;
using CommunityHub.Application.Services.Dtos.Apartments;

namespace CommunityHub.Application.Services.Apartments
{
    public class ApartmentService
    {
        private readonly IApartmentRepository _apartmentRepository
            = Injector.CreateInstance<IApartmentRepository>();
        private readonly ApartmentMapper _apartmentMapper = new();

        public long? FindIdByBuildingAndNumber(long buildingId, int apartmentNumber)
            => _apartmentRepository.FindIdByBuildingAndNumber(buildingId, apartmentNumber);  

        public List<ApartmentDto> GetByBuildingId(long buildingId)
        {
            var apartments = _apartmentRepository.GetByBuildingId(buildingId);
            var apartmentDtos = new List<ApartmentDto>();

            foreach (var apartment in apartments)
                apartmentDtos.Add(_apartmentMapper.MapToDto(apartment));

            return apartmentDtos;
        }
        
        public Apartment Create(Apartment apartment)
            => _apartmentRepository.Create(apartment);
    }
}
