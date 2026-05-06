namespace CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.TimeSlots
{
    public class AdTimeSlot : IEquatable<AdTimeSlot>
    {
        public long Id { get; }

        public long AdId { get; }
        public Ad? Ad { get; }

        public DateTime StartTime { get; }
        public DateTime EndTime { get; }

        public AdTimeSlotReservation? Reservation { get; }
        public bool IsReserved => Reservation is not null;

        public AdTimeSlot(
            long adId,
            DateTime startTime,
            DateTime endTime,
            Ad? ad = null,
            AdTimeSlotReservation? reservation = null,
            long id = 0
        ) {
            Id = id;

            AdId = adId;
            Ad = ad;

            StartTime = startTime;
            EndTime = endTime;
            Reservation = reservation;
        }

        public bool Matches(AdTimeSlot other)
        {
            if (Ad is null || other.Ad is null)
                throw new ArgumentException("Both AdTimeSlot instances must have a non-null Ad property.");

            if (other is null) return false;

            if (other.Id == Id) return false;

            if (other.AdId == AdId) return false;
            if (!other.Ad.Matches(Ad)) return false;

            if (other.EndTime != EndTime) return false;

            return other.StartTime == StartTime && other.EndTime == EndTime;
        }

        public AdTimeSlot CopyWith(
            long? id = null,
            long? adId = null,
            Ad? ad = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            AdTimeSlotReservation? reservation = null
        ) => new AdTimeSlot(
                    adId ?? AdId,
                    startTime ?? StartTime,
                    endTime ?? EndTime,
                    ad ?? Ad,
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
