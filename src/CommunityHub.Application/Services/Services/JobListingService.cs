using CommunityHub.Application.Domain.Services.RepositoryInterfaces;
using CommunityHub.Application.MappersDto.Services;
using CommunityHub.Application.Services.Dtos.Services;

namespace CommunityHub.Application.Services.Services
{
    public class JobListingService
    {
        private readonly IJobListingRepository _jobListingRepository
            = Injector.CreateInstance<IJobListingRepository>();
        private readonly JobListingMapper _jobListingMapper = new();

        public List<JobListingDto> GetByCityAndCountry(long cityId, long countryId)
            => _jobListingRepository.GetByCityAndCountry(cityId, countryId)
                .Select(_jobListingMapper.MapToDto)
                .ToList();
    }
}
