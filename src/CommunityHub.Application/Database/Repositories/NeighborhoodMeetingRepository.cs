using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain;

namespace CommunityHub.Application.Database.Repositories
{
    public class NeighborhoodMeetingRepository
    {
        private readonly NeighborhoodMeetingMapper _neighborhoodMeetingMapper = new NeighborhoodMeetingMapper();

        public List<NeighborhoodMeeting> GetByNeighborhoodId(long neighborhoodId)
        {
            var query = PostgresQuery.Create(
                @"SELECT 
                neighborhood_meetings.id AS meeting_id, 
                neighborhood_meetings.theme AS meeting_theme, 
                neighborhood_meetings.meeting_time, 
                neighborhood_meetings.date_range_start, 
                neighborhood_meetings.date_range_end, 
                neighborhood_meetings.scheduled_date, 
                neighborhood_meetings.status AS meeting_status,

                neighborhoods.id AS neighborhood_id, 
                neighborhoods.name AS neighborhood_name, 
                neighborhoods.description, 
                neighborhoods.budget,

                cities.id AS city_id, 
                cities.name AS city_name,

                countries.id AS country_id, 
                countries.name AS country_name,

                coordinator.id AS coordinator_user_id, 
                coordinator.username AS coordinator_username, 
                coordinator.password AS coordinator_password, 
                coordinator.name AS coordinator_name, 
                coordinator.surname AS coordinator_surname,     
                coordinator.user_role AS coordinator_user_role

                FROM neighborhood_meetings
                INNER JOIN neighborhoods ON neighborhood_meetings.neighborhood_id = neighborhoods.id
                INNER JOIN cities ON neighborhoods.city_id = cities.id
                INNER JOIN countries ON cities.country_id = countries.id
                INNER JOIN users AS coordinator ON neighborhoods.coordinator_id = coordinator.id
                WHERE neighborhood_meetings.neighborhood_id = @id"
            ).AddParameter("id", neighborhoodId);

            List<NeighborhoodMeeting> meetings = new List<NeighborhoodMeeting>();
            query.ExecuteReader(reader => {
                meetings.Add(_neighborhoodMeetingMapper.MapToEntity(reader));
            });
            return meetings;
        }
    }
}
