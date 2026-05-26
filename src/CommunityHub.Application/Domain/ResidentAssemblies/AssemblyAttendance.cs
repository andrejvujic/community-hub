using CommunityHub.Application.Domain.Apartments;

namespace CommunityHub.Application.Domain.ResidentAssemblies
{
    internal class AssemblyAttendance
    {
        public long Id { get; }

        public long AssemblyId { get; }
        public long ApartmentId { get; }
        public Apartment? Apartment { get; }

        public bool IsConfirmed { get; private set; } = false;

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

        public void Confirm()
        {
            if (IsConfirmed)
                throw new InvalidOperationException("Can't confirm assembly attendance twice");

            IsConfirmed = true;
        }

        public void Cancel()
        {
            if (!IsConfirmed)
                throw new InvalidOperationException("Can't cancel unconfirmed assembly attendance");

            IsConfirmed = false;
        }
    }
}
