namespace CommunityHub.Application.Domain.Buildings.Ads
{
    using System.Runtime.Serialization;

    public enum AdCategory
    {
        [EnumMember(Value = "moving")]
        Moving,

        [EnumMember(Value = "cleaning")]
        Cleaning,

        [EnumMember(Value = "repairs")]
        Repairs,

        [EnumMember(Value = "maintenance")]
        Maintenance,

        [EnumMember(Value = "borrowing")]
        Borrowing,

        [EnumMember(Value = "lending")]
        Lending,

        [EnumMember(Value = "delivery")]
        Delivery,

        [EnumMember(Value = "shopping")]
        Shopping,

        [EnumMember(Value = "tech_support")]
        TechSupport,

        [EnumMember(Value = "pet_care")]
        PetCare,

        [EnumMember(Value = "child_care")]
        ChildCare,

        [EnumMember(Value = "elderly_care")]
        ElderlyCare,

        [EnumMember(Value = "gardening")]
        Gardening,

        [EnumMember(Value = "tutoring")]
        Tutoring,

        [EnumMember(Value = "transport")]
        Transport
    }

    public static class AdCategoryValues
    {
        public static Array All => Enum.GetValues(typeof(AdCategory));
    }
}
