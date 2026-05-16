using CommunityHub.Application.Domain.ServiceBookings.RepositoryInterfaces;
using CommunityHub.Application.MappersDto.ServiceBookings;
using CommunityHub.Application.Services.Dtos.ServiceBookings;

namespace CommunityHub.Application.Services.ServiceBookings
{
    public class BookedJobService
    {
        private readonly IBookedJobRepository _bookedJobRepository
            = Injector.CreateInstance<IBookedJobRepository>();
        private readonly BookedJobMapper _bookedJobMapper = new();

        public List<BookedJobDto> GetByNeighborhood(long neighborhoodId)
            => _bookedJobRepository.GetByNeighborhoodId(neighborhoodId)
                .Select(_bookedJobMapper.MapToDto)
                .ToList();
    }
}
