using CommunityHub.Application.Database.Mapping;
using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain;

namespace CommunityHub.Application.Database.Repositories
{
    public class EventItemAssignmentRepository
    {
        private readonly EventItemAssignmentMapper _mapper = new EventItemAssignmentMapper();

        public List<EventItemAssignment> GetByEventId(long eventId)
        {
            var query = PostgresQuery.Create(
                @"SELECT
                a.id AS event_item_assignment_id,
                a.registration_id AS event_item_assignment_registration_id,
                a.event_item_id AS event_item_assignment_event_item_id,

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
                nb_coord.user_role AS registration_citizen_coordinator_user_role,

                ei.id AS event_item_id,
                ei.name AS event_item_name,
                ei.event_id AS event_item_event_id

                FROM event_item_assignments a
                INNER JOIN event_registrations r ON a.registration_id = r.id
                INNER JOIN citizens c ON r.citizen_id = c.id
                INNER JOIN addresses addr ON c.address_id = addr.id
                INNER JOIN streets st ON addr.street_id = st.id
                INNER JOIN cities ci ON addr.city_id = ci.id
                INNER JOIN countries co ON ci.country_id = co.id
                LEFT JOIN neighborhood_memberships mem ON c.membership_id = mem.id
                LEFT JOIN neighborhoods mem_nb ON mem.neighborhood_id = mem_nb.id
                LEFT JOIN users nb_coord ON mem_nb.coordinator_id = nb_coord.id
                INNER JOIN event_items ei ON a.event_item_id = ei.id
                WHERE ei.event_id = @event_id"
            ).AddParameter("event_id", eventId);

            var list = new List<EventItemAssignment>();
            query.ExecuteReader(reader =>
            {
                var assign = _mapper.MapToEntity(reader);
                if (assign != null) list.Add(assign);
            });

            return list;
        }

        public EventItemAssignment Create(EventItemAssignment assignment)
        {
            var query = PostgresQuery.Create(
                @"INSERT INTO event_item_assignments (registration_id, event_item_id)
                  VALUES (@registration_id, @event_item_id)
                  RETURNING id"
            ).ExtendParameters(_mapper.MapToQueryParameters(assignment));

            long id = query.ExecuteScalar<long>(value => Convert.ToInt64(value));
            return assignment.CopyWith(id: id);
        }

        public EventItemAssignment? GetById(long id)
        {
            var query = PostgresQuery.Create(
                @"SELECT
                a.id AS event_item_assignment_id,
                a.registration_id AS event_item_assignment_registration_id,
                a.event_item_id AS event_item_assignment_event_item_id,

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
                nb_coord.user_role AS registration_citizen_coordinator_user_role,

                ei.id AS event_item_id,
                ei.name AS event_item_name,
                ei.event_id AS event_item_event_id

                FROM event_item_assignments a
                INNER JOIN event_registrations r ON a.registration_id = r.id
                INNER JOIN citizens c ON r.citizen_id = c.id
                INNER JOIN addresses addr ON c.address_id = addr.id
                INNER JOIN streets st ON addr.street_id = st.id
                INNER JOIN cities ci ON addr.city_id = ci.id
                INNER JOIN countries co ON ci.country_id = co.id
                LEFT JOIN neighborhood_memberships mem ON c.membership_id = mem.id
                LEFT JOIN neighborhoods mem_nb ON mem.neighborhood_id = mem_nb.id
                LEFT JOIN users nb_coord ON mem_nb.coordinator_id = nb_coord.id
                INNER JOIN event_items ei ON a.event_item_id = ei.id
                WHERE a.id = @id"
            ).AddParameter("id", id);

            EventItemAssignment? result = null;
            query.ExecuteReader(reader =>
            {
                var assign = _mapper.MapToEntity(reader);
                if (assign != null) result = assign;
            });

            return result;
        }
    }
}
