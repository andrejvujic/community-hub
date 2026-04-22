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

                    city.id AS city_id,
                    city.name AS city_name,

                    country.id AS country_id,
                    country.name AS country_name,

                    organizer.id AS organizer_user_id,
                    organizer.username AS organizer_username,
                    organizer.password AS organizer_password,
                    organizer.name AS organizer_name,
                    organizer.surname AS organizer_surname,
                    organizer.user_role AS organizer_user_role

                  FROM community_events community_event
                  INNER JOIN neighborhoods neighborhood ON community_event.neighborhood_id = neighborhood.id
                  INNER JOIN cities city ON neighborhood.city_id = city.id
                  INNER JOIN countries country ON city.country_id = country.id
                  INNER JOIN users organizer ON community_event.organizer_citizen_id = organizer.id
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


    }
}
