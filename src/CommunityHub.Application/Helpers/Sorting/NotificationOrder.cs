namespace CommunityHub.Application.Helpers.Sorting
{
    public enum NotificationOrder
    {
        Unspecified,
        SentAtAscending,
        SentAtDescending,
    }
    public static class NotificationOrderValues
    {
        public static Array All => Enum.GetValues(typeof(NotificationOrder));
    }
}
