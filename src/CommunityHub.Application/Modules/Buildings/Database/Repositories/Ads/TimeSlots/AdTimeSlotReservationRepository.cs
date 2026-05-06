using CommunityHub.Application.Database;
using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.RepositoryInterfaces;
using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.TimeSlots;

namespace CommunityHub.Application.Modules.Buildings.Database.Repositories.Ads.TimeSlots
{
    public class AdTimeSlotReservationRepository : IAdTimeSlotReservationRepository
    {
        private readonly AdTimeSlotReservationMapper _mapper = new();
        public void Create(AdTimeSlotReservation reservation)
        {
            PostgresQuery.Create(
                @"INSERT INTO ad_time_slot_reservations (offer_ad_time_slot_id, request_ad_time_slot_id)
                  VALUES (@offer_ad_time_slot_id, @request_ad_time_slot_id)
                  RETURNING id"
            )
            .ExtendParameters(_mapper.MapToQueryParameters(reservation))
            .Execute();
        }

        public void CreateMany(List<AdTimeSlotReservation> reservations)
        {
            foreach (var reservation in reservations)
                Create(reservation);
        }
    }
}
