using System.Data;

namespace CommunityHub.Application.Database
{
    public static class IDataReaderExtensions
    {
        public static bool HasColumn(this IDataReader reader, string name)
        {
            for (int i = 0; i < reader.FieldCount; i++)
                if (reader.GetName(i).Equals(name, StringComparison.OrdinalIgnoreCase))
                    return true;
           
            return false;
        }

        public static PrefixedIDataReader WithPrefix(this IDataReader reader, string? prefix)
            => new PrefixedIDataReader(reader, prefix);
    }
}
