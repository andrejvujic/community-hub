using CommunityHub.Application.Domain;
using System.Data;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class EventItemMapper : EntityMapper<EventItem>
    {
        public override EventItem? MapToEntity(IDataReader reader)
        {
            if (reader.IsDBNull(reader.GetOrdinal("event_item_id"))) return null;
            long id = reader.GetInt64(reader.GetOrdinal("event_item_id"));

            string name = reader.GetString(reader.GetOrdinal("event_item_name"));
            long eventId = reader.GetInt64(reader.GetOrdinal("event_item_event_id"));

            return new EventItem(id, name, eventId);
        }

        public override Dictionary<string, object?> MapToQueryParameters(EventItem entity)
        {
            return new Dictionary<string, object?>
            {
                { "name", entity.Name },
                { "event_id", entity.EventId }
            };
        }
    }
}
