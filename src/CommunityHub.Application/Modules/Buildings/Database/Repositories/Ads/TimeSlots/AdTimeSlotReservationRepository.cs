using CommunityHub.Application.Database;
using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.RepositoryInterfaces;
using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.TimeSlots;
using System.Diagnostics;

namespace CommunityHub.Application.Modules.Buildings.Database.Repositories.Ads.TimeSlots
{
    public class AdTimeSlotReservationRepository : IAdTimeSlotReservationRepository
    {
        private readonly AdTimeSlotReservationMapper _mapper = new();

        public void Create(AdTimeSlotReservation reservation)
        {
            using var connection = DatabaseConnection.Create();

            connection.Execute(
                DatabaseQuery.Create(
                    @"INSERT INTO ad_time_slot_reservations
                        (offer_ad_time_slot_id, request_ad_time_slot_id)
                      VALUES
                        (@offer_ad_time_slot_id, @request_ad_time_slot_id)"
                )
                .ExtendParameters(
                    _mapper.MapToQueryParameters(reservation)
                )
            );
        }

        public void CreateMany(List<AdTimeSlotReservation> reservations)
        {
            using var connection = DatabaseConnection.Create();

            connection.BeginTransaction();

            try
            {
                foreach (var reservation in reservations)
                {
                    connection.Execute(
                        DatabaseQuery.Create(
                            @"INSERT INTO ad_time_slot_reservations
                            (offer_ad_time_slot_id, request_ad_time_slot_id)
                        VALUES
                            (@offer_ad_time_slot_id, @request_ad_time_slot_id)"
                        )
                        .ExtendParameters(
                            _mapper.MapToQueryParameters(reservation)
                        )
                    );
                }

                connection.CommitTransaction();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                connection.RollbackTranscation();
            }
        }
    }
}
