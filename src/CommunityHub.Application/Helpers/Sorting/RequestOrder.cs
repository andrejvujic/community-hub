namespace CommunityHub.Application.Helpers.Sorting
{
    public enum RequestOrder
    {
        Unspecified,
        CreationDateAscending,
        CreationDateDescending,
    }
    public static class RequestOrderValues
    {
        public static Array All => Enum.GetValues(typeof(RequestOrder));
    }
}
