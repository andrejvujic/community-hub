using CommunityHub.Application.Domain;
using System.Data;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class NeighborhoodMeetingMapper : EntityMapper<NeighborhoodMeeting>
    {
        private readonly NeighborhoodMapper _neighborhoodMapper = new NeighborhoodMapper();

        public override NeighborhoodMeeting MapToEntity(IDataReader reader)
        {
            long id = reader.GetInt64(reader.GetOrdinal("meeting_id"));

            MeetingTheme theme = (MeetingTheme)reader.GetValue(reader.GetOrdinal("meeting_theme"));
            int timeOrdinal = reader.GetOrdinal("meeting_time");
            TimeOnly time = reader.IsDBNull(timeOrdinal)
                ? TimeOnly.MinValue
                : (TimeOnly)reader.GetValue(timeOrdinal);
            DateOnly start = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("date_range_start")));
            DateOnly end = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("date_range_end")));
            DateOnly? scheduled = reader.IsDBNull(reader.GetOrdinal("scheduled_date"))
                ? null
                : DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("scheduled_date")));
            MeetingStatus status = (MeetingStatus)reader.GetValue(reader.GetOrdinal("meeting_status"));

            Neighborhood neighborhood = _neighborhoodMapper.MapToEntity(reader);

            return new NeighborhoodMeeting(neighborhood, theme, time, start, end, status, scheduled, id);
        }

        public override Dictionary<string, object?> MapToQueryParameters(NeighborhoodMeeting entity)
        {
            throw new NotImplementedException();
        }
    }
}