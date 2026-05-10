using CommunityHub.Application.Domain.Apartments.RepositoryInterfaces;

namespace CommunityHub.Application.Services.Apartments
{
    public class ApartmentService
    {
        private readonly IApartmentRepository _apartmentRepository
            = Injector.CreateInstance<IApartmentRepository>();  

        public long? FindIdByBuildingAndNumber(long buildingId, int apartmentNumber)
            => _apartmentRepository.FindIdByBuildingAndNumber(buildingId, apartmentNumber);       
    }
}
