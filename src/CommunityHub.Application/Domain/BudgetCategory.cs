using System.Runtime.Serialization;

namespace CommunityHub.Application.Domain
{
    public enum BudgetCategory
    {
        [EnumMember(Value = "street_maintenance")]
        StreetMaintenance,
        [EnumMember(Value = "park_maintenance")]
        ParkMaintenance,
        [EnumMember(Value = "lighting")]
        Lighting,
        [EnumMember(Value = "cleanliness")]
        Cleanliness,
        [EnumMember(Value = "greenery")]
        Greenery,
        [EnumMember(Value = "emergency_repairs")]
        EmergencyRepairs,
        [EnumMember(Value = "playgrounds")]
        Playgrounds,
        [EnumMember(Value = "sports_facilities")]
        SportsFacilities
    }
}