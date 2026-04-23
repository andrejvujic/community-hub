using CommunityHub.Application.Database.Mapping;
using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain.NoticeBoard;
using System.Diagnostics;

namespace CommunityHub.Application.Database.Repositories
{
    public class AdTimeSlotRepository
    {
        private readonly AdTimeSlotMapper _timeSlotMapper = new();

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
            ).ExtendParameters(_timeSlotMapper.MapToQueryParameters(adTimeSlot));

            long id = query.ExecuteScalar(value => Convert.ToInt64(value));

            return adTimeSlot.CopyWith(id: id);
        }

        public AdTimeSlot? GetMatchingTimeSlotInUserAds(long timeSlotId, long userId)
        {
            var query = PostgresQuery.Create(
                @"SELECT
                    time_slot_2.id AS time_slot_id,
                    time_slot_2.ad_id AS time_slot_ad_id,
                    time_slot_2.start_time,
                    time_slot_2.end_time,
                    EXISTS (SELECT 1 FROM ad_time_slot_reservations reservation WHERE
		                reservation.offer_ad_time_slot_id = time_slot_2.id OR
		                reservation.request_ad_time_slot_id = time_slot_2.id) AS is_reserved

                FROM ad_time_slots time_slot_1
                INNER JOIN ads ad_1 ON ad_1.id = time_slot_1.ad_id
                INNER JOIN ads ad_2 ON
	                ad_2.ad_category = ad_1.ad_category AND
	                ad_2.ad_type <> ad_1.ad_type AND
	                ad_2.resident_id <> ad_1.resident_id AND
	                ad_2.building_id = ad_1.building_id AND
	                ad_2.ad_status = 'active' AND
	                ad_2.resident_id = @user_id
                INNER JOIN ad_time_slots time_slot_2 ON
	                time_slot_2.ad_id = ad_2.id AND
	                time_slot_2.start_time = time_slot_1.start_time AND
	                time_slot_2.end_time = time_slot_1.end_time
                WHERE
	                time_slot_1.id = @time_slot_id AND
	                NOT EXISTS (SELECT 1 FROM ad_time_slot_reservations reservation WHERE
		                reservation.offer_ad_time_slot_id = time_slot_2.id OR
		                reservation.request_ad_time_slot_id = time_slot_2.id)"
            )
            .AddParameter("user_id", userId)
            .AddParameter("time_slot_id", timeSlotId);

            AdTimeSlot? timeSlot = null;
            query.ExecuteReader(reader => timeSlot = _timeSlotMapper.MapToEntity(reader));
            return timeSlot;
        }

        public List<AdTimeSlot> GetReservableTimeSlotsForUser(long adId, long userId)
        {
            var query = PostgresQuery.Create(
                @"SELECT
                    time_slot_1.id AS time_slot_id,
                    time_slot_1.ad_id AS time_slot_ad_id,
                    time_slot_1.start_time,
                    time_slot_1.end_time,
                    EXISTS (SELECT 1 FROM ad_time_slot_reservations reservation WHERE
		                reservation.offer_ad_time_slot_id = time_slot_1.id OR
		                reservation.request_ad_time_slot_id = time_slot_1.id) AS is_reserved

                FROM ad_time_slots time_slot_1
                INNER JOIN ads ad_1 ON ad_1.id = time_slot_1.ad_id
                INNER JOIN ads ad_2 ON
	                ad_2.ad_category = ad_1.ad_category AND
	                ad_2.ad_type <> ad_1.ad_type AND
	                ad_2.resident_id <> ad_1.resident_id AND
	                ad_2.building_id = ad_1.building_id AND
	                ad_2.ad_status = 'active' AND
	                ad_2.resident_id = @user_id
                INNER JOIN ad_time_slots time_slot_2 ON
	                time_slot_2.ad_id = ad_2.id AND
	                time_slot_2.start_time = time_slot_1.start_time AND
	                time_slot_2.end_time = time_slot_1.end_time
                WHERE
	                ad_1.id = @ad_id"
            )
            .AddParameter("user_id", userId)
            .AddParameter("ad_id", adId);

            var timeSlots = new List<AdTimeSlot>();

            query.ExecuteReader(
                reader => timeSlots.Add(
                    _timeSlotMapper.MapToEntity(reader)
                )
            );

            return timeSlots;
        }
    }
}
