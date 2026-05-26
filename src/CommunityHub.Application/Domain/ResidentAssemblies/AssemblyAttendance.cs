using CommunityHub.Application.Domain.Apartments;

namespace CommunityHub.Application.Domain.ResidentAssemblies
{
    public class AssemblyAttendance
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
    }
}
