using System.Data;

namespace CommunityHub.Application.Database
{
    public static class IDataReaderExtensions
    {
        public static int GetOrdinalPrefixed(this IDataReader reader, string columnName, string? columnPrefix)
        {
            if (columnPrefix == null)
                return reader.GetOrdinal(columnName);
            return reader.GetOrdinal($"{columnPrefix}_{columnName}");
        }
    }
}
