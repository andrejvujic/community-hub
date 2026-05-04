using CommunityHub.Application.Database;
using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.RepositoryInterfaces;
using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.TimeSlots;

namespace CommunityHub.Application.Modules.Buildings.Database.Repositories.Ads.TimeSlots
{
    public class AdTimeSlotRepository : IAdTimeSlotRepository
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

        public AdTimeSlot? GetMatchingTimeSlotInAuthorAds(long referenceTimeSlotId, long authorId)
        {
            var query = PostgresQuery.Create(
                @"SELECT
                    time_slot_2.id AS time_slot_id,
                    time_slot_2.ad_id AS time_slot_ad_id,
                    time_slot_2.start_time,
                    time_slot_2.end_time

                FROM ad_time_slots time_slot_1

                INNER JOIN ads ad_1 ON ad_1.id = time_slot_1.ad_id

                INNER JOIN ads ad_2 ON
	                ad_2.ad_category = ad_1.ad_category AND
	                ad_2.ad_type <> ad_1.ad_type AND
	                ad_2.resident_id <> ad_1.resident_id AND
	                ad_2.building_id = ad_1.building_id AND
	                ad_2.ad_status = 'active' AND
	                ad_2.resident_id = @author_id

                INNER JOIN ad_time_slots time_slot_2 ON
	                time_slot_2.ad_id = ad_2.id AND
	                time_slot_2.start_time = time_slot_1.start_time AND
	                time_slot_2.end_time = time_slot_1.end_time

                LEFT OUTER JOIN ad_time_slot_reservations reservation ON
                    (reservation.offer_ad_time_slot_id = time_slot_2.id) OR
                    (reservation.request_ad_time_slot_id = time_slot_2.id)

                WHERE
	                time_slot_1.id = @time_slot_id AND
	                reservation.id IS NULL"
            )
            .AddParameter("author_id", authorId)
            .AddParameter("time_slot_id", referenceTimeSlotId);

            AdTimeSlot? timeSlot = null;

            query.ExecuteReader(
                reader => timeSlot = _timeSlotMapper.MapToEntity(reader)
            );

            return timeSlot;
        }

        public List<AdTimeSlot> GetReservableTimeSlots(long adId, long userId)
        {
            var query = PostgresQuery.Create(
                @"SELECT
                    time_slot_1.id AS time_slot_id,
                    time_slot_1.ad_id AS time_slot_ad_id,
                    time_slot_1.start_time,
                    time_slot_1.end_time,

                    reservation.id as reservation_id,

                    reservation.offer_ad_time_slot_id,
                    reservation.request_ad_time_slot_id,

                    ad_1.id AS ad_1_ad_id,
                    ad_1.ad_category AS ad_1_ad_category,
                    ad_1.ad_status AS ad_1_ad_status,
                    ad_1.ad_type AS ad_1_ad_type,
                    ad_1.building_id AS ad_1_ad_building_id,
                    ad_1.description AS ad_1_description,
                    ad_1.start_date AS ad_1_start_date,
                    ad_1.end_date AS ad_1_end_date, 

                    author_1.id AS ad_1_author_user_id,
                    author_1.name AS ad_1_author_name,
                    author_1.surname AS ad_1_author_surname,
                    author_1.username AS ad_1_author_username,
                    author_1.password AS ad_1_author_password,
                    author_1.user_role AS ad_1_author_user_role,

                    ad_2.id AS ad_2_ad_id,
                    ad_2.ad_category AS ad_2_ad_category,
                    ad_2.ad_status AS ad_2_ad_status,
                    ad_2.ad_type AS ad_2_ad_type,
                    ad_2.building_id AS ad_2_ad_building_id,
                    ad_2.description AS ad_2_description,
                    ad_2.start_date AS ad_2_start_date,
                    ad_2.end_date AS ad_2_end_date, 

                    author_2.id AS ad_2_author_user_id,
                    author_2.name AS ad_2_author_name,
                    author_2.surname AS ad_2_author_surname,
                    author_2.username AS ad_2_author_username,
                    author_2.password AS ad_2_author_password,
                    author_2.user_role AS ad_2_author_user_role

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
                
                LEFT OUTER JOIN ad_time_slot_reservations reservation ON
                    (reservation.offer_ad_time_slot_id = time_slot_1.id) OR
                    (reservation.request_ad_time_slot_id = time_slot_1.id)
               
                INNER JOIN users author_1 ON author_1.id = ad_1.resident_id

                INNER JOIN users author_2 ON author_2.id = ad_2.resident_id

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

        public List<AdTimeSlot> GetReservedTimeSlots(long adId)
        {
            var query = PostgresQuery.Create(
                @"SELECT
                    time_slot_1.id AS time_slot_id,
                    time_slot_1.ad_id AS time_slot_ad_id,
                    time_slot_1.start_time,
                    time_slot_1.end_time,

                    reservation.id as reservation_id,

                    reservation.offer_ad_time_slot_id,
                    reservation.request_ad_time_slot_id,

                    ad_1.id AS ad_1_ad_id,
                    ad_1.ad_category AS ad_1_ad_category,
                    ad_1.ad_status AS ad_1_ad_status,
                    ad_1.ad_type AS ad_1_ad_type,
                    ad_1.building_id AS ad_1_ad_building_id,
                    ad_1.description AS ad_1_description,
                    ad_1.start_date AS ad_1_start_date,
                    ad_1.end_date AS ad_1_end_date, 

                    author_1.id AS ad_1_author_user_id,
                    author_1.name AS ad_1_author_name,
                    author_1.surname AS ad_1_author_surname,
                    author_1.username AS ad_1_author_username,
                    author_1.password AS ad_1_author_password,
                    author_1.user_role AS ad_1_author_user_role,

                    ad_2.id AS ad_2_ad_id,
                    ad_2.ad_category AS ad_2_ad_category,
                    ad_2.ad_status AS ad_2_ad_status,
                    ad_2.ad_type AS ad_2_ad_type,
                    ad_2.building_id AS ad_2_ad_building_id,
                    ad_2.description AS ad_2_description,
                    ad_2.start_date AS ad_2_start_date,
                    ad_2.end_date AS ad_2_end_date, 

                    author_2.id AS ad_2_author_user_id,
                    author_2.name AS ad_2_author_name,
                    author_2.surname AS ad_2_author_surname,
                    author_2.username AS ad_2_author_username,
                    author_2.password AS ad_2_author_password,
                    author_2.user_role AS ad_2_author_user_role

                FROM ad_time_slots time_slot_1

                INNER JOIN ads ad_1 ON ad_1.id = time_slot_1.ad_id

                INNER JOIN ads ad_2 ON
	                ad_2.ad_category = ad_1.ad_category AND
	                ad_2.ad_type <> ad_1.ad_type AND
	                ad_2.resident_id <> ad_1.resident_id AND
	                ad_2.building_id = ad_1.building_id

                INNER JOIN ad_time_slots time_slot_2 ON
	                time_slot_2.ad_id = ad_2.id AND
	                time_slot_2.start_time = time_slot_1.start_time AND
	                time_slot_2.end_time = time_slot_1.end_time
                
                INNER JOIN ad_time_slot_reservations reservation ON
                    (reservation.offer_ad_time_slot_id = time_slot_1.id) OR
                    (reservation.request_ad_time_slot_id = time_slot_1.id)
               
                INNER JOIN users author_1 ON author_1.id = ad_1.resident_id

                INNER JOIN users author_2 ON author_2.id = ad_2.resident_id

                WHERE
	                ad_1.id = @ad_id"
            )
            .AddParameter("ad_id", adId);

            List<AdTimeSlot> timeSlots = new();

            query.ExecuteReader(
                reader => timeSlots.Add(
                    _timeSlotMapper.MapToEntity(reader)
                )
            );

            return timeSlots;
        }
    }
}
