using CommunityHub.Application.Domain.RepositoryInterfaces;

namespace CommunityHub.Application.Modules.Buildings.Core.Services.Apartments
{
    public class ApartmentService
    {
        private readonly IApartmentRepository _apartmentRepository
            = Injector.CreateInstance<IApartmentRepository>();  

        public long? FindIdByBuildingAndNumber(long buildingId, int apartmentNumber)
            => _apartmentRepository.FindIdByBuildingAndNumber(buildingId, apartmentNumber);       
    }
}
