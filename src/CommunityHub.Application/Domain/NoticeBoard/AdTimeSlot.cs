using System.Xml.Linq;

namespace CommunityHub.Application.Domain.NoticeBoard
{
    public class AdTimeSlot : IEquatable<AdTimeSlot>
    {
        public long Id { get; }
        public long AdId { get; }
        public DateTime StartTime { get; }
        public DateTime EndTime { get; }

        public AdTimeSlotReservation? Reservation { get; }
        public bool IsReserved => Reservation != null;

        public AdTimeSlot(
            long adId,
            DateTime startTime,
            DateTime endTime,
            AdTimeSlotReservation? reservation = null,
            long id = 0
        ) {
            Id = id;
            AdId = adId;
            StartTime = startTime;
            EndTime = endTime;
            Reservation = reservation;
        }

        public AdTimeSlot CopyWith(
            long? id = null,
            long? adId = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            AdTimeSlotReservation? reservation = null
        ) => new AdTimeSlot(
                    adId ?? AdId,
                    startTime ?? StartTime,
                    endTime ?? EndTime,
                    reservation ?? Reservation,
                    id ?? Id
                 );

        public bool Equals(AdTimeSlot? other)
        {
            if (other is null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return Id == other.Id &&
                AdId == other.AdId &&
                StartTime.Equals(other.StartTime) &&
                EndTime.Equals(other.EndTime);
        }
        public override bool Equals(object? other)
        {
            if (other is null || other is not AdTimeSlot) return false;
            return Equals(other as AdTimeSlot);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, AdId, StartTime, EndTime);
        }

        public static bool operator ==(AdTimeSlot? left, AdTimeSlot? right)
        {
            if (left is null)
                return right is null;

            return left.Equals(right);
        }

        public static bool operator !=(AdTimeSlot? left, AdTimeSlot? right)
        {
            return !(left == right);
        }
    }
}
