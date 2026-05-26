using CommunityHub.Application.Domain.ResidentAssemblies;

namespace CommunityHub.Application.Services.Dtos.ResidentAssemblies
{
    public class AssemblyDto
    {
        public long Id { get; set; }
        public long BuildingId { get; set; }
        public int BuildingApartmentCount { get; set; }
        public DateTime StartsAt { get; set; }
        public AssemblyStatus Status { get; set; }

        public AssemblyDto(
            long id,
            long buildingId,
            int buildingApartmentCount,
            DateTime startsAt,
            AssemblyStatus status
        )
        {
            Id = id;
            BuildingId = buildingId;
            BuildingApartmentCount = buildingApartmentCount;
            StartsAt = startsAt;
            Status = status;
        }
    }
}
