using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain.Events;
using CommunityHub.Application.Domain.Events.RepositoryInterfaces;

namespace CommunityHub.Application.Database.Repositories
{
    public class EventItemRepository : IEventItemRepository
    {
        private readonly EventItemMapper _mapper = new EventItemMapper();

        public EventItem Create(EventItem item)
        {
            var query = PostgresQuery.Create(
                @"INSERT INTO event_items (name, event_id)
                  VALUES (@name, @event_id)
                  RETURNING id"
            ).ExtendParameters(_mapper.MapToQueryParameters(item));

            long id = query.ExecuteScalar<long>(value => Convert.ToInt64(value));
            return item.CopyWith(id: id);
        }

        public List<EventItem> GetByEventId(long eventId)
        {
            var query = PostgresQuery.Create(
                @"SELECT
                id AS event_item_id,
                name AS event_item_name,
                event_id AS event_item_event_id
                FROM event_items
                WHERE event_id = @event_id"
            ).AddParameter("event_id", eventId);

            var items = new List<EventItem>();
            query.ExecuteReader(reader =>
            {
                var item = _mapper.MapToEntity(reader);
                if (item is not null) items.Add(item);
            });

            return items;
        }
    }
}
