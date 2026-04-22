using CommunityHub.Application.Domain;
using System;
using System.Data;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class CommunityEventMapper : EntityMapper<CommunityEvent>
    {
        private readonly NeighborhoodMapper _neighborhoodMapper = new NeighborhoodMapper();
        private readonly CitizenMapper _citizenMapper = new CitizenMapper();

        public override CommunityEvent? MapToEntity(IDataReader reader)
        {
            if (reader.IsDBNull(reader.GetOrdinal("community_event_id"))) return null;
            long id = reader.GetInt64(reader.GetOrdinal("community_event_id"));

            string name = reader.GetString(reader.GetOrdinal("community_event_name"));

            string? description = null;
            if (!reader.IsDBNull(reader.GetOrdinal("community_event_description")))
                description = reader.GetString(reader.GetOrdinal("community_event_description"));

            DateTime datePart = reader.GetDateTime(reader.GetOrdinal("community_event_event_date"));
            var timeValue = reader.GetValue(reader.GetOrdinal("community_event_start_time"));
            DateTime eventStart = timeValue is TimeSpan ts
                ? datePart.Date + ts
                : datePart.Date + ((DateTime)timeValue).TimeOfDay;

            int durationMinutes = reader.GetInt32(reader.GetOrdinal("community_event_duration_minutes"));
            int minVolunteers = reader.GetInt32(reader.GetOrdinal("community_event_min_volunteers"));
            EventStatus status = (EventStatus)reader.GetValue(reader.GetOrdinal("community_event_status"));

            Neighborhood? neighborhood = _neighborhoodMapper.MapToEntity(reader.WithPrefix("neighborhood"));
            Citizen? organizer = _citizenMapper.MapToEntity(reader.WithPrefix("organizer"));

            return new CommunityEvent(id, name, description, eventStart, durationMinutes, minVolunteers, status, neighborhood!, organizer!);
        }

        public override Dictionary<string, object?> MapToQueryParameters(CommunityEvent entity)
        {
            return new Dictionary<string, object?>
            {
                { "name", entity.Name },
                { "description", entity.Description },
                { "event_date", entity.EventStart.Date },
                { "start_time", entity.EventStart.TimeOfDay },
                { "duration_minutes", entity.DurationMinutes },
                { "min_volunteers", entity.MinVolunteers },
                { "status", entity.Status },
                { "neighborhood_id", entity.Neighborhood?.Id },
                { "organizer_citizen_id", entity.Organizer?.Id }
            };
        }
    }
}
