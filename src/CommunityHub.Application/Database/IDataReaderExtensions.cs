using System.Data;

namespace CommunityHub.Application.Database
{
    public static class IDataReaderExtensions
    {
        public static int GetOrdinalPrefixed(
            this IDataReader reader,
            string columnName,
            string? columnNamePrefix = null
        )
        {
            if (columnNamePrefix == null)
                return reader.GetOrdinal(columnName);

            return reader.GetOrdinal($"{columnNamePrefix}_{columnName}");
        }

        public static bool HasColumn(
            this IDataReader reader,
            string columnName,
            string? columnNamePrefix = null
        )
        {
            string targetColumnName = columnNamePrefix == null ? columnName : $"{columnNamePrefix}_{columnName}";

            for (int i = 0; i < reader.FieldCount; i++)
                if (reader.GetName(i).Equals(targetColumnName, StringComparison.OrdinalIgnoreCase))
                    return true;
                
            return false;
        }
    }
}
