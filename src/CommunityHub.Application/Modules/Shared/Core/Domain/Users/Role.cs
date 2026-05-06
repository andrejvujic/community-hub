using System.Runtime.Serialization;

namespace CommunityHub.Application.Modules.Shared.Core.Domain.Users
{
    public enum Role
    {
        [EnumMember(Value = "building_manager")]
        BuildingManager,

        [EnumMember(Value = "building_resident")]
        BuildingResident,

        [EnumMember(Value = "neighborhood_coordinator")]
        NeighborhoodCoordinator,

        [EnumMember(Value = "citizen")]
        Citizen,

        [EnumMember(Value = "neighborhood_service_provider")]
        NeighborhoodServiceProvider
    }
}
