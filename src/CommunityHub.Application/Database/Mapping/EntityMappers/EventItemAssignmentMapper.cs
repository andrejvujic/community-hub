using CommunityHub.Application.Domain;
using System.Data;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class EventItemAssignmentMapper : EntityMapper<EventItemAssignment>
    {
        private readonly EventRegistrationMapper? _registrationMapper = new EventRegistrationMapper();
        private readonly EventItemMapper? _itemMapper = new EventItemMapper();

        public override EventItemAssignment? MapToEntity(IDataReader reader)
        {
            if (reader.IsDBNull(reader.GetOrdinal("event_item_assignment_id"))) return null;
            long id = reader.GetInt64(reader.GetOrdinal("event_item_assignment_id"));

            EventRegistration registration = _registrationMapper?.MapToEntity(reader) ?? throw new Exception("Couldn't read EventItemAssignment from database because of missing registration data");
            EventItem item = _itemMapper?.MapToEntity(reader) ?? throw new Exception("Couldn't read EventItemAssignment from database because of missing item data");

            return new EventItemAssignment(id, registration, item);
        }

        public override Dictionary<string, object?> MapToQueryParameters(EventItemAssignment entity)
        {
            return new Dictionary<string, object?>
            {
                { "registration_id", entity.Registration.Id },
                { "event_item_id", entity.Item.Id }
            };
        }
    }
}
