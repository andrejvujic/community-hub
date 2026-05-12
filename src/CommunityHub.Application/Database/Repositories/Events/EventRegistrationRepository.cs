using CommunityHub.Application.Database.Mapping;
using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain.Events;
using CommunityHub.Application.Domain.Events.RepositoryInterfaces;

namespace CommunityHub.Application.Database.Repositories.Events
{
    public class EventRegistrationRepository : IEventRegistrationRepository
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
                c.user_role AS registration_citizen_user_role,

                addr.id AS registration_citizen_address_id,
                addr.street_number AS registration_citizen_street_number,
                addr.neighborhood AS registration_citizen_neighborhood,

                st.id AS registration_citizen_street_id,
                st.name AS registration_citizen_street_name,

                ci.id AS registration_citizen_city_id,
                ci.name AS registration_citizen_city_name,

                co.id AS registration_citizen_country_id,
                co.name AS registration_citizen_country_name,

                mem.id AS registration_citizen_membership_id,
                mem.acceptance_date AS registration_citizen_acceptance_date,

                mem_nb.id AS registration_citizen_neighborhood_id,
                mem_nb.name AS registration_citizen_neighborhood_name,
                mem_nb.description AS registration_citizen_description,
                mem_nb.budget AS registration_citizen_budget,

                nb_coord.id AS registration_citizen_coordinator_user_id,
                nb_coord.username AS registration_citizen_coordinator_username,
                nb_coord.password AS registration_citizen_coordinator_password,
                nb_coord.name AS registration_citizen_coordinator_name,
                nb_coord.surname AS registration_citizen_coordinator_surname,
                nb_coord.user_role AS registration_citizen_coordinator_user_role

                FROM event_registrations r
                INNER JOIN citizens c ON r.citizen_id = c.id
                INNER JOIN addresses addr ON c.address_id = addr.id
                INNER JOIN streets st ON addr.street_id = st.id
                INNER JOIN cities ci ON addr.city_id = ci.id
                INNER JOIN countries co ON ci.country_id = co.id
                LEFT JOIN neighborhood_memberships mem ON c.membership_id = mem.id
                LEFT JOIN neighborhoods mem_nb ON mem.neighborhood_id = mem_nb.id
                LEFT JOIN users nb_coord ON mem_nb.coordinator_id = nb_coord.id
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

        public void UpdateAttendance(long registrationId, bool? attended)
        {
            var query = PostgresQuery.Create(
                "UPDATE event_registrations SET attended = @attended WHERE id = @id"
            )
            .AddParameter("id", registrationId)
            .AddParameter("attended", attended);

            query.GetRowsAffectedCount();
        }

        public int GetConfirmedVolunteerCount(long citizenId)
        {
            var query = PostgresQuery.Create(
                "SELECT COUNT(*) FROM event_registrations WHERE citizen_id = @citizenId AND attended = TRUE"
            ).AddParameter("citizenId", citizenId);

            return query.ExecuteScalar<int>(Convert.ToInt32);
        }
    }
}
