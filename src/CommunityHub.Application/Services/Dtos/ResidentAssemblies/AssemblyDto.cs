using CommunityHub.Application.Domain.Buildings;
using CommunityHub.Application.Domain.ResidentAssemblies;
using System.Diagnostics;

namespace CommunityHub.Application.Services.Dtos.ResidentAssemblies
{
    public class AssemblyDto
    {
        public long Id { get; set; }
        public long BuildingId { get; set; }
        public int BuildingApartmentCount { get; set; }
        public DateTime StartsAt { get; set; }
        public AssemblyStatus Status { get; set; }
        public List<AssemblyAttendanceDto> Attendances { get; set; }

        public AssemblyDto(
            long buildingId,
            int buildingApartmentCount,
            DateTime startsAt,
            AssemblyStatus status,
            List<AssemblyAttendanceDto>? attendances = null,
            long id = 0
        )
        {
            Id = id;
            BuildingId = buildingId;
            BuildingApartmentCount = buildingApartmentCount;
            StartsAt = startsAt;
            Status = status;
            Attendances = attendances ?? new();
        }
        public static AssemblyDto Create(Building building, DateTime startsAt)
        {
            return new AssemblyDto(
                building.Id,
                building.ApartmentCount,
                startsAt,
                AssemblyStatus.Scheduled
            );
        }

        public const double AttendancePercentageThreshold = 0.5;

        public double AttendancePercentage
            => BuildingApartmentCount > 0 ? Attendances.Count / (double)BuildingApartmentCount : 0.0;
    }
}
