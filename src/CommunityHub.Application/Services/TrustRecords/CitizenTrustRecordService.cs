using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Database.Repositories.Events;
using CommunityHub.Application.Domain.Events.RepositoryInterfaces;
using CommunityHub.Application.Domain.TrustRecords;
using CommunityHub.Application.Domain.Users;
using CommunityHub.Application.MappersDto;
using CommunityHub.Application.MappersDto.TrustRecords;
using CommunityHub.Application.MappersDto.Users;
using CommunityHub.Application.Services.Dtos.TrustRecords;
using CommunityHub.Application.Services.Dtos.Users;
using CommunityHub.Application.Services.Users;

namespace CommunityHub.Application.Services.TrustRecords
{
    public class CitizenTrustRecordService
    {
        private readonly CitizenService _citizenService = new();
        private readonly ICommunityEventRepository _communityEventRepository = new CommunityEventRepository();
        private readonly IEventRegistrationRepository _eventRegistrationRepository = new EventRegistrationRepository();
        private readonly IMapper<CitizenTrustRecord, CitizenTrustRecordDto> _trustRecordMapper = new CitizenTrustRecordMapper();
        private readonly IMapper<Citizen, CitizenDto> _citizenMapper = new CitizenMapper();

        public CitizenTrustRecordDto? GetTrustRecord(long userId)
        {
            var citizen = _citizenService.GetCitizenById(userId);
            if (citizen == null) return null;

            int organized = _communityEventRepository.GetOrganizedEventsCount(userId);
            int volunteered = _eventRegistrationRepository.GetConfirmedVolunteerCount(userId);

            var citizenDomain = _citizenMapper.MapToDomain(citizen);
            var trustRecord = new CitizenTrustRecord(citizenDomain, organized, volunteered);

            return _trustRecordMapper.MapToDto(trustRecord);
        }

        public long? GetNeighborhoodIdForUser(long userId)
        {
            var citizen = _citizenService.GetCitizenById(userId);
            return citizen?.Membership?.Neighborhood.Id;
        }
    }
}
