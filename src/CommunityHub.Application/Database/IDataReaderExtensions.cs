using System.Data;

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

        public static bool HasColumn(this IDataReader reader, string columnName, string? columnNamePrefix)
        {
            string columnNameWithPrefix = columnNamePrefix == null ? columnName : $"{columnNamePrefix}_{columnName}";

            for (int i = 0; i < reader.FieldCount; i++)
                if (reader.GetName(i).Equals(columnNameWithPrefix, StringComparison.OrdinalIgnoreCase))
                    return true;
                
            return false;
        }
    }
}
