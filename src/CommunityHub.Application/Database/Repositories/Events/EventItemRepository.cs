using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain.Events;
using CommunityHub.Application.Domain.RepositoryInterfaces.Events;

namespace CommunityHub.Application.Database.Repositories.Events
{
    public class EventItemRepository : IEventItemRepository
    {
        private readonly EventItemMapper _mapper = new EventItemMapper();

        public EventItem Create(EventItem item)
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery.Create(
                @"INSERT INTO event_items (name, event_id)
                  VALUES (@name, @event_id)
                  RETURNING id"
            ).ExtendParameters(_mapper.MapToQueryParameters(item));

            long id = connection.ExecuteScalar<long>(query, value => Convert.ToInt64(value));
            return item.CopyWith(id: id);
        }

        public List<EventItem> GetByEventId(long eventId)
        {
            using var connection = DatabaseConnection.Create();
            return GetByEventId(eventId, connection);
        }

        internal List<EventItem> GetByEventId(long eventId, DatabaseConnection connection)
        {
            var query = DatabaseQuery.Create(
                @"SELECT
                id AS event_item_id,
                name AS event_item_name,
                event_id AS event_item_event_id
                FROM event_items
                WHERE event_id = @event_id"
            ).AddParameter("event_id", eventId);

            var items = new List<EventItem>();
            connection.ExecuteReader(query, reader =>
            {
                var item = _mapper.MapToEntity(reader);
                if (item is not null) items.Add(item);
            });

            return items;
        }

        internal List<EventItem> GetByEventIds(IEnumerable<long> eventIds, DatabaseConnection connection)
        {
            if (!eventIds.Any()) return new();

            var idsList = eventIds.ToList();
            var placeholders = string.Join(", ", idsList.Select((_, i) => $"@id{i}"));
            var query = DatabaseQuery.Create(
                $@"SELECT
                id AS event_item_id,
                name AS event_item_name,
                event_id AS event_item_event_id
                FROM event_items
                WHERE event_id IN ({placeholders})"
            );

            for (int i = 0; i < idsList.Count; i++)
                query.AddParameter($"id{i}", idsList[i]);

            var items = new List<EventItem>();
            connection.ExecuteReader(query, reader =>
            {
                var item = _mapper.MapToEntity(reader);
                if (item is not null) items.Add(item);
            });

            return items;
        }
    }
}
