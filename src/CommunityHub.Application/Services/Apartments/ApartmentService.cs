using CommunityHub.Application.Domain.Apartments;
using CommunityHub.Application.Domain.RepositoryInterfaces.Apartments;

namespace CommunityHub.Application.Services.Apartments
{
    public class ApartmentService
    {
        private readonly IApartmentRepository _apartmentRepository
            = Injector.CreateInstance<IApartmentRepository>();  

        public long? FindIdByBuildingAndNumber(long buildingId, int apartmentNumber)
            => _apartmentRepository.FindIdByBuildingAndNumber(buildingId, apartmentNumber);  
        
        public Apartment Create(Apartment apartment)
            => _apartmentRepository.Create(apartment);
    }
}
