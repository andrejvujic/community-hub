using CommunityHub.Application.Database;
using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.RepositoryInterfaces;

namespace CommunityHub.Application.Modules.Buildings.Database.Repositories.Ads.TimeSlots
{
    public class AdTimeSlotReservationRepository : IAdTimeSlotReservationRepository
    {
        public void Create(long offerAdTimeSlotId, long requestAdTimeSlotId)
        {
            PostgresQuery.Create(
                @"INSERT INTO ad_time_slot_reservations (offer_ad_time_slot_id, request_ad_time_slot_id)
                  VALUES (@offer_ad_time_slot_id, @request_ad_time_slot_id)
                  RETURNING id"
            )
            .AddParameter("offer_ad_time_slot_id", offerAdTimeSlotId)
            .AddParameter("request_ad_time_slot_id", requestAdTimeSlotId)
            .Execute();
        }
    }
}
