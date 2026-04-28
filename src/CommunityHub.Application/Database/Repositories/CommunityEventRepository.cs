using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain;

namespace CommunityHub.Application.Database.Repositories
{
    public class CommunityEventRepository
    {
        private readonly CommunityEventMapper _eventMapper = new CommunityEventMapper();
        private readonly EventItemMapper _itemMapper = new EventItemMapper();

        public CommunityEvent Create(CommunityEvent communityEvent)
        {
            var query = PostgresQuery.Create(
                @"INSERT INTO community_events (name, description, event_date, start_time, duration_minutes, min_volunteers, status, neighborhood_id, organizer_citizen_id)
                  VALUES (@name, @description, @event_date, @start_time, @duration_minutes, @min_volunteers, @status, @neighborhood_id, @organizer_citizen_id)
                  RETURNING id"
            ).ExtendParameters(_eventMapper.MapToQueryParameters(communityEvent));

            long eventId = query.ExecuteScalar<long>(value => Convert.ToInt64(value));

            var createdItems = new List<EventItem>();

            var itemRepo = new EventItemRepository();
            foreach (var item in communityEvent.Items)
            {
                var created = itemRepo.Create(item.CopyWith(eventId: eventId));
                createdItems.Add(created);
            }

            return communityEvent.CopyWith(id: eventId, items: createdItems);
        }

        public List<CommunityEvent> GetByOrganizerId(long organizerId)
        {
            var query = PostgresQuery.Create(
                @"SELECT
                community_event.id AS community_event_id,
                community_event.name AS community_event_name,
                community_event.description AS community_event_description,
                community_event.event_date AS community_event_event_date,
                community_event.start_time AS community_event_start_time,
                community_event.duration_minutes AS community_event_duration_minutes,
                community_event.min_volunteers AS community_event_min_volunteers,
                community_event.status AS community_event_status,

                neighborhood.id AS neighborhood_id,
                neighborhood.name AS neighborhood_name,
                neighborhood.description,
                neighborhood.budget,

                coordinator.id AS coordinator_user_id,
                coordinator.username AS coordinator_username,
                coordinator.password AS coordinator_password,
                coordinator.name AS coordinator_name,
                coordinator.surname AS coordinator_surname,
                coordinator.user_role AS coordinator_user_role,

                city.id AS city_id,
                city.name AS city_name,

                country.id AS country_id,
                country.name AS country_name,

                organizer.id AS organizer_user_id,
                organizer.username AS organizer_username,
                organizer.password AS organizer_password,
                organizer.name AS organizer_name,
                organizer.surname AS organizer_surname,
                organizer.user_role AS organizer_user_role,

                organizer_address.id AS organizer_address_id,
                organizer_address.street_number AS organizer_street_number,
                organizer_address.neighborhood AS organizer_neighborhood,

                organizer_street.id AS organizer_street_id,
                organizer_street.name AS organizer_street_name,

                organizer_city.id AS organizer_city_id,
                organizer_city.name AS organizer_city_name,

                organizer_country.id AS organizer_country_id,
                organizer_country.name AS organizer_country_name,

                organizer_membership.id AS organizer_membership_id,
                organizer_membership.acceptance_date AS organizer_acceptance_date,

                organizer_neighborhood.id AS organizer_neighborhood_id,
                organizer_neighborhood.name AS organizer_neighborhood_name,
                organizer_neighborhood.description AS organizer_neighborhood_description,
                organizer_neighborhood.budget AS organizer_neighborhood_budget,

                organizer_neighborhood.description AS organizer_description,
                organizer_neighborhood.budget AS organizer_budget,

                organizer_neighborhood_coordinator.id AS organizer_coordinator_user_id,
                organizer_neighborhood_coordinator.username AS organizer_coordinator_username,
                organizer_neighborhood_coordinator.password AS organizer_coordinator_password,
                organizer_neighborhood_coordinator.name AS organizer_coordinator_name,
                organizer_neighborhood_coordinator.surname AS organizer_coordinator_surname,
                organizer_neighborhood_coordinator.user_role AS organizer_coordinator_user_role

                FROM community_events community_event
                INNER JOIN neighborhoods neighborhood ON community_event.neighborhood_id = neighborhood.id
                INNER JOIN cities city ON neighborhood.city_id = city.id
                INNER JOIN countries country ON city.country_id = country.id
                LEFT JOIN users coordinator ON neighborhood.coordinator_id = coordinator.id
                INNER JOIN citizens organizer ON community_event.organizer_citizen_id = organizer.id
                INNER JOIN addresses organizer_address ON organizer.address_id = organizer_address.id
                INNER JOIN streets organizer_street ON organizer_address.street_id = organizer_street.id
                INNER JOIN cities organizer_city ON organizer_address.city_id = organizer_city.id
                INNER JOIN countries organizer_country ON organizer_city.country_id = organizer_country.id
                LEFT JOIN neighborhood_memberships organizer_membership ON organizer.membership_id = organizer_membership.id
                LEFT JOIN neighborhoods organizer_neighborhood ON organizer_membership.neighborhood_id = organizer_neighborhood.id
                LEFT JOIN users organizer_neighborhood_coordinator ON organizer_neighborhood.coordinator_id = organizer_neighborhood_coordinator.id
                WHERE community_event.organizer_citizen_id = @organizerId"
            ).AddParameter("organizerId", organizerId);

            var events = new List<CommunityEvent>();
            query.ExecuteReader(reader => {
                var ev = _eventMapper.MapToEntity(reader);
                if (ev != null) events.Add(ev);
            });

            var itemRepo = new EventItemRepository();
            var regRepo = new EventRegistrationRepository();
            var assignRepo = new EventItemAssignmentRepository();

            var result = new List<CommunityEvent>();
            foreach (var ev in events)
            {
                var items = itemRepo.GetByEventId(ev.Id);
                var regs = regRepo.GetByEventId(ev.Id);
                var assigns = assignRepo.GetByEventId(ev.Id);

                result.Add(ev.CopyWith(items: items, registrations: regs, assignments: assigns));
            }

            return result;
        }

        public List<CommunityEvent> GetByVolunteerCitizenId(long citizenId)
        {
            var query = PostgresQuery.Create(
                @"SELECT
                community_event.id AS community_event_id,
                community_event.name AS community_event_name,
                community_event.description AS community_event_description,
                community_event.event_date AS community_event_event_date,
                community_event.start_time AS community_event_start_time,
                community_event.duration_minutes AS community_event_duration_minutes,
                community_event.min_volunteers AS community_event_min_volunteers,
                community_event.status AS community_event_status,

                neighborhood.id AS neighborhood_id,
                neighborhood.name AS neighborhood_name,
                neighborhood.description,
                neighborhood.budget,

                coordinator.id AS coordinator_user_id,
                coordinator.username AS coordinator_username,
                coordinator.password AS coordinator_password,
                coordinator.name AS coordinator_name,
                coordinator.surname AS coordinator_surname,
                coordinator.user_role AS coordinator_user_role,

                city.id AS city_id,
                city.name AS city_name,

                country.id AS country_id,
                country.name AS country_name,

                organizer.id AS organizer_user_id,
                organizer.username AS organizer_username,
                organizer.password AS organizer_password,
                organizer.name AS organizer_name,
                organizer.surname AS organizer_surname,
                organizer.user_role AS organizer_user_role,

                organizer_address.id AS organizer_address_id,
                organizer_address.street_number AS organizer_street_number,
                organizer_address.neighborhood AS organizer_neighborhood,

                organizer_street.id AS organizer_street_id,
                organizer_street.name AS organizer_street_name,

                organizer_city.id AS organizer_city_id,
                organizer_city.name AS organizer_city_name,

                organizer_country.id AS organizer_country_id,
                organizer_country.name AS organizer_country_name,

                organizer_membership.id AS organizer_membership_id,
                organizer_membership.acceptance_date AS organizer_acceptance_date,

                organizer_neighborhood.id AS organizer_neighborhood_id,
                organizer_neighborhood.name AS organizer_neighborhood_name,
                organizer_neighborhood.description AS organizer_neighborhood_description,
                organizer_neighborhood.budget AS organizer_neighborhood_budget,

                organizer_neighborhood.description AS organizer_description,
                organizer_neighborhood.budget AS organizer_budget,

                organizer_neighborhood_coordinator.id AS organizer_coordinator_user_id,
                organizer_neighborhood_coordinator.username AS organizer_coordinator_username,
                organizer_neighborhood_coordinator.password AS organizer_coordinator_password,
                organizer_neighborhood_coordinator.name AS organizer_coordinator_name,
                organizer_neighborhood_coordinator.surname AS organizer_coordinator_surname,
                organizer_neighborhood_coordinator.user_role AS organizer_coordinator_user_role

                FROM community_events community_event
                INNER JOIN event_registrations r ON r.event_id = community_event.id
                INNER JOIN neighborhoods neighborhood ON community_event.neighborhood_id = neighborhood.id
                INNER JOIN cities city ON neighborhood.city_id = city.id
                INNER JOIN countries country ON city.country_id = country.id
                LEFT JOIN users coordinator ON neighborhood.coordinator_id = coordinator.id
                INNER JOIN citizens organizer ON community_event.organizer_citizen_id = organizer.id
                INNER JOIN addresses organizer_address ON organizer.address_id = organizer_address.id
                INNER JOIN streets organizer_street ON organizer_address.street_id = organizer_street.id
                INNER JOIN cities organizer_city ON organizer_address.city_id = organizer_city.id
                INNER JOIN countries organizer_country ON organizer_city.country_id = organizer_country.id
                LEFT JOIN neighborhood_memberships organizer_membership ON organizer.membership_id = organizer_membership.id
                LEFT JOIN neighborhoods organizer_neighborhood ON organizer_membership.neighborhood_id = organizer_neighborhood.id
                LEFT JOIN users organizer_neighborhood_coordinator ON organizer_neighborhood.coordinator_id = organizer_neighborhood_coordinator.id
                WHERE r.citizen_id = @citizenId"
            ).AddParameter("citizenId", citizenId);

            var events = new List<CommunityEvent>();
            query.ExecuteReader(reader => {
                var ev = _eventMapper.MapToEntity(reader);
                if (ev != null) events.Add(ev);
            });

            var itemRepo = new EventItemRepository();
            var regRepo = new EventRegistrationRepository();
            var assignRepo = new EventItemAssignmentRepository();

            var result = new List<CommunityEvent>();
            foreach (var ev in events)
            {
                var items = itemRepo.GetByEventId(ev.Id);
                var regs = regRepo.GetByEventId(ev.Id);
                var assigns = assignRepo.GetByEventId(ev.Id);

                result.Add(ev.CopyWith(items: items, registrations: regs, assignments: assigns));
            }

            return result;
        }

        public List<CommunityEvent> GetByNeighborhoodId(long neighborhoodId)
        {
            var query = PostgresQuery.Create(
                @"SELECT
                community_event.id AS community_event_id,
                community_event.name AS community_event_name,
                community_event.description AS community_event_description,
                community_event.event_date AS community_event_event_date,
                community_event.start_time AS community_event_start_time,
                community_event.duration_minutes AS community_event_duration_minutes,
                community_event.min_volunteers AS community_event_min_volunteers,
                community_event.status AS community_event_status,

                neighborhood.id AS neighborhood_id,
                neighborhood.name AS neighborhood_name,
                neighborhood.description,
                neighborhood.budget,

                coordinator.id AS coordinator_user_id,
                coordinator.username AS coordinator_username,
                coordinator.password AS coordinator_password,
                coordinator.name AS coordinator_name,
                coordinator.surname AS coordinator_surname,
                coordinator.user_role AS coordinator_user_role,

                city.id AS city_id,
                city.name AS city_name,

                country.id AS country_id,
                country.name AS country_name,

                organizer.id AS organizer_user_id,
                organizer.username AS organizer_username,
                organizer.password AS organizer_password,
                organizer.name AS organizer_name,
                organizer.surname AS organizer_surname,
                organizer.user_role AS organizer_user_role,

                organizer_address.id AS organizer_address_id,
                organizer_address.street_number AS organizer_street_number,
                organizer_address.neighborhood AS organizer_neighborhood,

                organizer_street.id AS organizer_street_id,
                organizer_street.name AS organizer_street_name,

                organizer_city.id AS organizer_city_id,
                organizer_city.name AS organizer_city_name,

                organizer_country.id AS organizer_country_id,
                organizer_country.name AS organizer_country_name,

                organizer_membership.id AS organizer_membership_id,
                organizer_membership.acceptance_date AS organizer_acceptance_date,

                organizer_neighborhood.id AS organizer_neighborhood_id,
                organizer_neighborhood.name AS organizer_neighborhood_name,
                organizer_neighborhood.description AS organizer_neighborhood_description,
                organizer_neighborhood.budget AS organizer_neighborhood_budget,

                organizer_neighborhood.description AS organizer_description,
                organizer_neighborhood.budget AS organizer_budget,

                organizer_neighborhood_coordinator.id AS organizer_coordinator_user_id,
                organizer_neighborhood_coordinator.username AS organizer_coordinator_username,
                organizer_neighborhood_coordinator.password AS organizer_coordinator_password,
                organizer_neighborhood_coordinator.name AS organizer_coordinator_name,
                organizer_neighborhood_coordinator.surname AS organizer_coordinator_surname,
                organizer_neighborhood_coordinator.user_role AS organizer_coordinator_user_role

                FROM community_events community_event
                INNER JOIN neighborhoods neighborhood ON community_event.neighborhood_id = neighborhood.id
                INNER JOIN cities city ON neighborhood.city_id = city.id
                INNER JOIN countries country ON city.country_id = country.id
                LEFT JOIN users coordinator ON neighborhood.coordinator_id = coordinator.id
                INNER JOIN citizens organizer ON community_event.organizer_citizen_id = organizer.id
                INNER JOIN addresses organizer_address ON organizer.address_id = organizer_address.id
                INNER JOIN streets organizer_street ON organizer_address.street_id = organizer_street.id
                INNER JOIN cities organizer_city ON organizer_address.city_id = organizer_city.id
                INNER JOIN countries organizer_country ON organizer_city.country_id = organizer_country.id
                LEFT JOIN neighborhood_memberships organizer_membership ON organizer.membership_id = organizer_membership.id
                LEFT JOIN neighborhoods organizer_neighborhood ON organizer_membership.neighborhood_id = organizer_neighborhood.id
                LEFT JOIN users organizer_neighborhood_coordinator ON organizer_neighborhood.coordinator_id = organizer_neighborhood_coordinator.id
                WHERE community_event.neighborhood_id = @neighborhoodId"
            ).AddParameter("neighborhoodId", neighborhoodId);

            var events = new List<CommunityEvent>();
            query.ExecuteReader(reader => {
                var ev = _eventMapper.MapToEntity(reader);
                if (ev != null) events.Add(ev);
            });

            var itemRepo = new EventItemRepository();
            var regRepo = new EventRegistrationRepository();
            var assignRepo = new EventItemAssignmentRepository();

            var result = new List<CommunityEvent>();
            foreach (var ev in events)
            {
                var items = itemRepo.GetByEventId(ev.Id);
                var regs = regRepo.GetByEventId(ev.Id);
                var assigns = assignRepo.GetByEventId(ev.Id);

                result.Add(ev.CopyWith(items: items, registrations: regs, assignments: assigns));
            }

            return result;
        }

        public CommunityEvent? GetById(long eventId)
        {
            var query = PostgresQuery.Create(
                @"SELECT
                    community_event.id AS community_event_id,
                    community_event.name AS community_event_name,
                    community_event.description AS community_event_description,
                    community_event.event_date AS community_event_event_date,
                    community_event.start_time AS community_event_start_time,
                    community_event.duration_minutes AS community_event_duration_minutes,
                    community_event.min_volunteers AS community_event_min_volunteers,
                    community_event.status AS community_event_status,

                    neighborhood.id AS neighborhood_id,
                    neighborhood.name AS neighborhood_name,
                    neighborhood.description,
                    neighborhood.budget,

                    coordinator.id AS coordinator_user_id,
                    coordinator.username AS coordinator_username,
                    coordinator.password AS coordinator_password,
                    coordinator.name AS coordinator_name,
                    coordinator.surname AS coordinator_surname,
                    coordinator.user_role AS coordinator_user_role,

                    city.id AS city_id,
                    city.name AS city_name,

                    country.id AS country_id,
                    country.name AS country_name,

                    organizer.id AS organizer_user_id,
                    organizer.username AS organizer_username,
                    organizer.password AS organizer_password,
                    organizer.name AS organizer_name,
                    organizer.surname AS organizer_surname,
                    organizer.user_role AS organizer_user_role,

                    organizer_address.id AS organizer_address_id,
                    organizer_address.street_number AS organizer_street_number,
                    organizer_address.neighborhood AS organizer_neighborhood,

                    organizer_street.id AS organizer_street_id,
                    organizer_street.name AS organizer_street_name,

                    organizer_city.id AS organizer_city_id,
                    organizer_city.name AS organizer_city_name,

                    organizer_country.id AS organizer_country_id,
                    organizer_country.name AS organizer_country_name,

                    organizer_membership.id AS organizer_membership_id,
                    organizer_membership.acceptance_date AS organizer_acceptance_date,

                    organizer_neighborhood.id AS organizer_neighborhood_id,
                    organizer_neighborhood.name AS organizer_neighborhood_name,
                    organizer_neighborhood.description AS organizer_neighborhood_description,
                    organizer_neighborhood.budget AS organizer_neighborhood_budget,

                    organizer_neighborhood.description AS organizer_description,
                    organizer_neighborhood.budget AS organizer_budget,

                    organizer_neighborhood_coordinator.id AS organizer_coordinator_user_id,
                    organizer_neighborhood_coordinator.username AS organizer_coordinator_username,
                    organizer_neighborhood_coordinator.password AS organizer_coordinator_password,
                    organizer_neighborhood_coordinator.name AS organizer_coordinator_name,
                    organizer_neighborhood_coordinator.surname AS organizer_coordinator_surname,
                    organizer_neighborhood_coordinator.user_role AS organizer_coordinator_user_role

                FROM community_events community_event
                INNER JOIN neighborhoods neighborhood ON community_event.neighborhood_id = neighborhood.id
                INNER JOIN cities city ON neighborhood.city_id = city.id
                INNER JOIN countries country ON city.country_id = country.id
                LEFT JOIN users coordinator ON neighborhood.coordinator_id = coordinator.id
                INNER JOIN citizens organizer ON community_event.organizer_citizen_id = organizer.id
                INNER JOIN addresses organizer_address ON organizer.address_id = organizer_address.id
                INNER JOIN streets organizer_street ON organizer_address.street_id = organizer_street.id
                INNER JOIN cities organizer_city ON organizer_address.city_id = organizer_city.id
                INNER JOIN countries organizer_country ON organizer_city.country_id = organizer_country.id
                LEFT JOIN neighborhood_memberships organizer_membership ON organizer.membership_id = organizer_membership.id
                LEFT JOIN neighborhoods organizer_neighborhood ON organizer_membership.neighborhood_id = organizer_neighborhood.id
                LEFT JOIN users organizer_neighborhood_coordinator ON organizer_neighborhood.coordinator_id = organizer_neighborhood_coordinator.id
                WHERE community_event.id = @eventId"
            ).AddParameter("eventId", eventId);

            CommunityEvent? ev = null;
            query.ExecuteReader(reader => {
                var mapped = _eventMapper.MapToEntity(reader);
                if (mapped != null) ev = mapped;
            });

            if (ev == null) return null;

            var itemRepo = new EventItemRepository();
            var regRepo = new EventRegistrationRepository();
            var assignRepo = new EventItemAssignmentRepository();
            var items = itemRepo.GetByEventId(ev.Id);
            var regs = regRepo.GetByEventId(ev.Id);
            var assigns = assignRepo.GetByEventId(ev.Id);
            return ev.CopyWith(items: items, registrations: regs, assignments: assigns);
        }

        public void UpdateStatus(long eventId, EventStatus status)
        {
            var query = PostgresQuery.Create(
                "UPDATE community_events SET status = @status WHERE id = @id"
            ).AddParameter("status", status)
             .AddParameter("id", eventId);
            query.GetRowsAffectedCount();
        }

        public int GetOrganizedEventsCount(long organizerId)
        {
            var query = PostgresQuery.Create(
                "SELECT COUNT(*) FROM community_events WHERE organizer_citizen_id = @organizerId AND status = @status"
            )
            .AddParameter("organizerId", organizerId)
            .AddParameter("status", EventStatus.Finished);

            return query.ExecuteScalar<int>(Convert.ToInt32);
        }
    }
}
