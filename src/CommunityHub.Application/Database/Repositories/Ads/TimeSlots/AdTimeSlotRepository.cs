using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain.Ads.RepositoryInterfaces;
using CommunityHub.Application.Domain.Ads.TimeSlots;
using System.Diagnostics;

namespace CommunityHub.Application.Database.Repositories.Ads.TimeSlots
{
    public class AdTimeSlotRepository : IAdTimeSlotRepository
    {
        private readonly AdTimeSlotMapper _timeSlotMapper = new();

        public void Create(AdTimeSlot timeSlot)
        {
            using var connection = DatabaseConnection.Create();

            connection.Execute(
                DatabaseQuery.Create(
                    @"INSERT INTO ad_time_slots
                        (ad_id,
                        start_time,
                        end_time)
                    VALUES
                        (@ad_id,
                        @start_time,
                        @end_time)"
                ).ExtendParameters(
                    _timeSlotMapper.MapToQueryParameters(timeSlot)
                )
            );
        }

        public void CreateMany(List<AdTimeSlot> timeSlots)
        {
            using var connection = DatabaseConnection.Create();

            connection.BeginTransaction();

            try
            {
                foreach (var timeSlot in timeSlots)
                    connection.Execute(
                        DatabaseQuery.Create(
                            @"INSERT INTO ad_time_slots
                                (ad_id,
                                start_time,
                                end_time)
                            VALUES
                                (@ad_id,
                                @start_time,
                                @end_time)"
                        ).ExtendParameters(
                            _timeSlotMapper.MapToQueryParameters(timeSlot)
                        )
                    );

                connection.CommitTransaction();
            } catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                connection.RollbackTranscation();
            }
        }

        public List<AdTimeSlot> GetTimeSlotsByAuthor(long authorId)
        {
            using var connection = DatabaseConnection.Create();
            var query = DatabaseQuery.Create(
                @"SELECT
                    time_slot.id AS time_slot_id,
                    time_slot.ad_id AS time_slot_ad_id,
                    time_slot.start_time,
                    time_slot.end_time,

                    ad.id AS ad_id,
                    ad.resident_id AS author_id,
                    ad.ad_category,
                    ad.ad_status,
                    ad.ad_type,
                    ad.building_id AS ad_building_id,
                    ad.description,
                    ad.start_date,
                    ad.end_date,

                    author.id AS author_user_id,
                    author.name AS author_name,
                    author.surname AS author_surname,
                    author.username AS author_username,
                    author.password AS author_password,
                    author.user_role AS author_user_role,

                    reservation.id AS reservation_id,
                    reservation.offer_ad_time_slot_id, 
                    reservation.request_ad_time_slot_id,

                    offer_ad.id AS offer_ad_ad_id,
                    offer_ad.resident_id AS offer_ad_author_id,
                    offer_ad.ad_category AS offer_ad_ad_category,
                    offer_ad.ad_status AS offer_ad_ad_status,
                    offer_ad.ad_type AS offer_ad_ad_type,
                    offer_ad.building_id AS offer_ad_ad_building_id,
                    offer_ad.description AS offer_ad_description,
                    offer_ad.start_date AS offer_ad_start_date,
                    offer_ad.end_date AS offer_ad_end_date,

                    offer_ad_author.id AS offer_ad_author_user_id,
                    offer_ad_author.name AS offer_ad_author_name,
                    offer_ad_author.surname AS offer_ad_author_surname,
                    offer_ad_author.username AS offer_ad_author_username,
                    offer_ad_author.password AS offer_ad_author_password,
                    offer_ad_author.user_role AS offer_ad_author_user_role,

                    request_ad.id AS request_ad_ad_id,
                    request_ad.resident_id AS request_ad_author_id,
                    request_ad.ad_category AS request_ad_ad_category,
                    request_ad.ad_status AS request_ad_ad_status,
                    request_ad.ad_type AS request_ad_ad_type,
                    request_ad.building_id AS request_ad_ad_building_id,
                    request_ad.description AS request_ad_description,
                    request_ad.start_date AS request_ad_start_date,
                    request_ad.end_date AS request_ad_end_date,

                    request_ad_author.id AS request_ad_author_user_id,
                    request_ad_author.name AS request_ad_author_name,
                    request_ad_author.surname AS request_ad_author_surname,
                    request_ad_author.username AS request_ad_author_username,
                    request_ad_author.password AS request_ad_author_password,
                    request_ad_author.user_role AS request_ad_author_user_role

                FROM ad_time_slots time_slot

                INNER JOIN ads ad ON ad.id = time_slot.ad_id
                INNER JOIN users author ON author.id = ad.resident_id

                LEFT OUTER JOIN ad_time_slot_reservations reservation ON
                    (reservation.offer_ad_time_slot_id = time_slot.id) OR
                    (reservation.request_ad_time_slot_id = time_slot.id)

                LEFT OUTER JOIN ad_time_slots offer_time_slot ON offer_time_slot.id = reservation.offer_ad_time_slot_id
                LEFT OUTER JOIN ad_time_slots request_time_slot ON request_time_slot.id = reservation.request_ad_time_slot_id
                LEFT OUTER JOIN ads offer_ad ON offer_ad.id = offer_time_slot.ad_id
                LEFT OUTER JOIN users offer_ad_author ON offer_ad_author.id = offer_ad.resident_id
                LEFT OUTER JOIN ads request_ad ON request_ad.id = request_time_slot.ad_id
                LEFT OUTER JOIN users request_ad_author ON request_ad_author.id = request_ad.resident_id

                WHERE
                    ad.resident_id = @author_id"
            )
            .AddParameter("author_id", authorId);

            var timeSlots = new List<AdTimeSlot>();

            connection.ExecuteReader(
                query,
                reader => { 
                    var timeSlot = _timeSlotMapper.MapToEntity(reader);
                    if (timeSlot is null) return;
                    timeSlots.Add(timeSlot);
                }
            );

            return timeSlots;
        }

        public List<AdTimeSlot> GetTimeSlotsByAd(long adId)
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery.Create(
                @"SELECT
                    time_slot.id AS time_slot_id,
                    time_slot.ad_id AS time_slot_ad_id,
                    time_slot.start_time,
                    time_slot.end_time,

                    ad.id AS ad_id,
                    ad.resident_id AS author_id,
                    ad.ad_category,
                    ad.ad_status,
                    ad.ad_type,
                    ad.building_id AS ad_building_id,
                    ad.description,
                    ad.start_date,
                    ad.end_date,

                    author.id AS author_user_id,
                    author.name AS author_name,
                    author.surname AS author_surname,
                    author.username AS author_username,
                    author.password AS author_password,
                    author.user_role AS author_user_role,

                    reservation.id AS reservation_id,
                    reservation.offer_ad_time_slot_id, 
                    reservation.request_ad_time_slot_id,

                    offer_ad.id AS offer_ad_ad_id,
                    offer_ad.resident_id AS offer_ad_author_id,
                    offer_ad.ad_category AS offer_ad_ad_category,
                    offer_ad.ad_status AS offer_ad_ad_status,
                    offer_ad.ad_type AS offer_ad_ad_type,
                    offer_ad.building_id AS offer_ad_ad_building_id,
                    offer_ad.description AS offer_ad_description,
                    offer_ad.start_date AS offer_ad_start_date,
                    offer_ad.end_date AS offer_ad_end_date,

                    offer_ad_author.id AS offer_ad_author_user_id,
                    offer_ad_author.name AS offer_ad_author_name,
                    offer_ad_author.surname AS offer_ad_author_surname,
                    offer_ad_author.username AS offer_ad_author_username,
                    offer_ad_author.password AS offer_ad_author_password,
                    offer_ad_author.user_role AS offer_ad_author_user_role,

                    request_ad.id AS request_ad_ad_id,
                    request_ad.resident_id AS request_ad_author_id,
                    request_ad.ad_category AS request_ad_ad_category,
                    request_ad.ad_status AS request_ad_ad_status,
                    request_ad.ad_type AS request_ad_ad_type,
                    request_ad.building_id AS request_ad_ad_building_id,
                    request_ad.description AS request_ad_description,
                    request_ad.start_date AS request_ad_start_date,
                    request_ad.end_date AS request_ad_end_date,

                    request_ad_author.id AS request_ad_author_user_id,
                    request_ad_author.name AS request_ad_author_name,
                    request_ad_author.surname AS request_ad_author_surname,
                    request_ad_author.username AS request_ad_author_username,
                    request_ad_author.password AS request_ad_author_password,
                    request_ad_author.user_role AS request_ad_author_user_role

                FROM ad_time_slots time_slot

                INNER JOIN ads ad ON ad.id = time_slot.ad_id
                INNER JOIN users author ON author.id = ad.resident_id

                LEFT OUTER JOIN ad_time_slot_reservations reservation ON
                    (reservation.offer_ad_time_slot_id = time_slot.id) OR
                    (reservation.request_ad_time_slot_id = time_slot.id)

                LEFT OUTER JOIN ad_time_slots offer_time_slot ON offer_time_slot.id = reservation.offer_ad_time_slot_id
                LEFT OUTER JOIN ad_time_slots request_time_slot ON request_time_slot.id = reservation.request_ad_time_slot_id
                LEFT OUTER JOIN ads offer_ad ON offer_ad.id = offer_time_slot.ad_id
                LEFT OUTER JOIN users offer_ad_author ON offer_ad_author.id = offer_ad.resident_id
                LEFT OUTER JOIN ads request_ad ON request_ad.id = request_time_slot.ad_id
                LEFT OUTER JOIN users request_ad_author ON request_ad_author.id = request_ad.resident_id

                WHERE
                    ad.id = @ad_id"
            )
            .AddParameter("ad_id", adId);

            var timeSlots = new List<AdTimeSlot>();

            connection.ExecuteReader(
                query,
                reader => {
                    var timeSlot = _timeSlotMapper.MapToEntity(reader);
                    if (timeSlot is null) return;
                    timeSlots.Add(timeSlot);
                }
            );

            return timeSlots;
        }
    }
}
