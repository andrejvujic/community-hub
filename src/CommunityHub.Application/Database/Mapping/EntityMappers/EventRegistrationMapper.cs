using CommunityHub.Application.Domain;
using System.Data;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class EventRegistrationMapper : EntityMapper<EventRegistration>
    {
        private readonly CitizenMapper? _citizenMapper = new CitizenMapper();

        public override EventRegistration? MapToEntity(IDataReader reader)
        {
            if (reader.IsDBNull(reader.GetOrdinal("registration_id"))) return null;
            long id = reader.GetInt64(reader.GetOrdinal("registration_id"));

            Citizen? citizen = (_citizenMapper?.MapToEntity(reader.WithPrefix("registration_citizen"))) ?? throw new Exception("Couldn't read EventRegistration from database because of missing citizen data");

            bool? attended = null;
            int attendedOrd = reader.GetOrdinal("registration_attended");
            if (!reader.IsDBNull(attendedOrd)) attended = reader.GetBoolean(attendedOrd);

            long eventId = reader.GetInt64(reader.GetOrdinal("registration_event_id"));

            return new EventRegistration(id, citizen, eventId, attended);
        }

        public override Dictionary<string, object?> MapToQueryParameters(EventRegistration entity)
        {
            return new Dictionary<string, object?>
            {
                { "attended", entity.Attended },
                { "event_id", entity.EventId },
                { "citizen_id", entity.Citizen.Id }
            };
        }
    }
}
