using CommunityHub.Application.Domain.Apartments;

namespace CommunityHub.Application.Domain.ResidentAssemblies
{
    public class AssemblyAttendance : IEquatable<AssemblyAttendance>
    {
        public long Id { get; }

        public long AssemblyId { get; }

        public long ApartmentId { get; }
        public Apartment? Apartment { get; }

        public AssemblyAttendance(
            long assemblyId,
            long apartmentId,
            Apartment? apartment = null,
            long id = 0
        )
        {
            AssemblyId = assemblyId;
            ApartmentId = apartmentId;
            Apartment = apartment;
            Id = id;
        }

        public bool Equals(AssemblyAttendance? other)
        {
            if (other is null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return AssemblyId == other.AssemblyId
                && ApartmentId == other.ApartmentId;
        }

        public override bool Equals(object? obj)
            => Equals(obj as AssemblyAttendance);

        public override int GetHashCode()
            => HashCode.Combine(AssemblyId, ApartmentId);

        public static bool operator ==(
            AssemblyAttendance? left,
            AssemblyAttendance? right
        )
            => EqualityComparer<AssemblyAttendance>.Default.Equals(left, right);

        public static bool operator !=(
            AssemblyAttendance? left,
            AssemblyAttendance? right
        )
            => !(left == right);
    }
}