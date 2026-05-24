using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Database.Repositories.Events;
using CommunityHub.Application.Domain.Events;
using CommunityHub.Application.Domain.RepositoryInterfaces.Events;
using CommunityHub.Application.Domain.RepositoryInterfaces.Users;
using CommunityHub.Application.MappersDto;
using CommunityHub.Application.MappersDto.Events;
using CommunityHub.Application.Services.Dtos.Events;

namespace CommunityHub.Application.Services.Events
{
    public class EventRegistrationService
    {
        private readonly IEventRegistrationRepository _registrationRepository = new EventRegistrationRepository();
        private readonly ICitizenRepository _citizenRepository = new CitizenRepository();
        private readonly IMapper<EventRegistration, EventRegistrationDto> _registrationMapper = new EventRegistrationMapper();

        public List<EventRegistrationDto> GetByEventId(long eventId)
            => _registrationRepository.GetByEventId(eventId)
                .Select(_registrationMapper.MapToDto)
                .ToList();

        public bool IsAlreadyRegistered(long citizenId, long eventId)
        {
            var registrations = _registrationRepository.GetByEventId(eventId);
            return registrations.Any(r => r.Citizen.Id == citizenId);
        }

        public EventRegistrationDto? CreateRegistration(long citizenId, long eventId)
        {
            var citizen = _citizenRepository.GetById(citizenId);
            if (citizen == null)
                return null;

            var registration = new EventRegistration(0, citizen, eventId, null);
            var created = _registrationRepository.Create(registration);
            return _registrationMapper.MapToDto(created);
        }

        public void UpdateAttendance(long registrationId, bool? attended)
        {
            _registrationRepository.UpdateAttendance(registrationId, attended);
        }

        public int GetConfirmedVolunteerCount(long citizenId)
            => _registrationRepository.GetConfirmedVolunteerCount(citizenId);
    }
}
