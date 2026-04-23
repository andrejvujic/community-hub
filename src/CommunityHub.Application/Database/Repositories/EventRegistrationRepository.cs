using CommunityHub.Application.Database.Mapping;
using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain;

namespace CommunityHub.Application.Database.Repositories
{
    public class EventRegistrationRepository
    {
        private readonly EventRegistrationMapper _mapper = new EventRegistrationMapper();

        public List<EventRegistration> GetByEventId(long eventId)
        {
            var query = PostgresQuery.Create(
                @"SELECT
                r.id AS registration_id,
                r.attended AS registration_attended,
                r.event_id AS registration_event_id,

                c.id AS registration_citizen_user_id,
                c.username AS registration_citizen_username,
                c.password AS registration_citizen_password,
                c.name AS registration_citizen_name,
                c.surname AS registration_citizen_surname,
                c.user_role AS registration_citizen_user_role
          
                FROM event_registrations r
                INNER JOIN users c ON r.citizen_id = c.id
                WHERE r.event_id = @event_id"
            ).AddParameter("event_id", eventId);

            var items = new List<EventRegistration>();
            query.ExecuteReader(reader =>
            {
                var reg = _mapper.MapToEntity(reader);
                if (reg is not null) items.Add(reg);
            });

            return items;
        }

        public EventRegistration Create(EventRegistration registration)
        {
            var query = PostgresQuery.Create(
                @"INSERT INTO event_registrations (attended, event_id, citizen_id)
                  VALUES (@attended, @event_id, @citizen_id)
                  RETURNING id"
            ).ExtendParameters(_mapper.MapToQueryParameters(registration));

            long id = query.ExecuteScalar<long>(value => Convert.ToInt64(value));
            return registration.CopyWith(id: id);
        }

        public EventRegistration? GetById(long id)
        {
            var query = PostgresQuery.Create(
                @"SELECT
                r.id AS registration_id,
                r.attended AS registration_attended,
                r.event_id AS registration_event_id,

                c.id AS registration_citizen_user_id,
                c.username AS registration_citizen_username,
                c.password AS registration_citizen_password,
                c.name AS registration_citizen_name,
                c.surname AS registration_citizen_surname,
                c.user_role AS registration_citizen_user_role
                
                FROM event_registrations r
                INNER JOIN users c ON r.citizen_id = c.id
                WHERE r.id = @id"
            ).AddParameter("id", id);

            EventRegistration? result = null;
            query.ExecuteReader(reader =>
            {
                var reg = _mapper.MapToEntity(reader);
                if (reg != null) result = reg;
            });

            return result;
        }
    }
}
