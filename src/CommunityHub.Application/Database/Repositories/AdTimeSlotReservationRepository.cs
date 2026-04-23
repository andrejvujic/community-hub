using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain.NoticeBoard;

namespace CommunityHub.Application.Database.Repositories
{
    public class AdTimeSlotReservationRepository
    {
        private readonly AdTimeSlotReservationMapper _timeSlotReservationMapper = new();

        public AdTimeSlotReservation Create(AdTimeSlotReservation reservation)
        {
            var query = PostgresQuery.Create(
                @"INSERT INTO ad_time_slot_reservations (offer_ad_time_slot_id, request_ad_time_slot_id)
                  VALUES (@first_time_slot_id, @second_time_slot_id)
                  RETURNING id"
            )
            .ExtendParameters(_timeSlotReservationMapper.MapToQueryParameters(reservation));

            long id = query.ExecuteScalar(value => Convert.ToInt64(value));

            return reservation.CopyWith(id: id);
        }
    }
}
