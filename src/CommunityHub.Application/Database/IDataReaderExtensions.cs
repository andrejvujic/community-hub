using System.Data;
using System.Diagnostics;

namespace CommunityHub.Application.Database
{
    public static class IDataReaderExtensions
    {
        public static int GetOrdinalPrefixed(this IDataReader reader, string columnName, string? columnNamePrefix)
        {
            if (columnNamePrefix == null)
                return reader.GetOrdinal(columnName);

            return reader.GetOrdinal($"{columnNamePrefix}_{columnName}");
        }
    }
}
