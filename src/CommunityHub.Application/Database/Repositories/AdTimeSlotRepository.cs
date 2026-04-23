using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain.NoticeBoard;

namespace CommunityHub.Application.Database.Repositories
{
    public class AdTimeSlotRepository
    {
        private readonly AdTimeSlotMapper _mapper = new();

        public AdTimeSlot Create(AdTimeSlot adTimeSlot)
        {
            var query = PostgresQuery.Create(
                @"INSERT INTO ad_time_slots
                    (ad_id,
                    start_time,
                    end_time)
                VALUES
                    (@ad_id,
                    @start_time,
                    @end_time)
                RETURNING id"
            ).ExtendParameters(_mapper.MapToQueryParameters(adTimeSlot));

            long id = query.ExecuteScalar(value => Convert.ToInt64(value));

            return adTimeSlot.CopyWith(id: id);
        }
    }
}
