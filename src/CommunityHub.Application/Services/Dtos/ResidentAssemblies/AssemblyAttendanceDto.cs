using CommunityHub.Application.Services.Dtos.Apartments;

namespace CommunityHub.Application.Services.Dtos.ResidentAssemblies
{
    public class AssemblyAttendanceDto
    {
        public long Id { get; set; }
        public long AssemblyId { get; set; }
        public long ApartmentId { get; set; }
        public ApartmentDto? Apartment { get; set; }

        public AssemblyAttendanceDto(
            long id,
            long assemblyId,
            long apartmentId,
            ApartmentDto? apartment = null
        )
        {
            Id = id;
            AssemblyId = assemblyId;
            ApartmentId = apartmentId;
            Apartment = apartment;
        }
    }
}
