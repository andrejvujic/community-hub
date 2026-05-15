using System.Data;

namespace CommunityHub.Application.Database.Mapping
{
    public static class IDataReaderExtensions
    {
        public static bool HasColumn(this IDataReader reader, string name)
        {
            if (reader is PrefixedIDataReader prefixedReader)
                return prefixedReader.HasColumn(name);  

            for (int i = 0; i < reader.FieldCount; i++)
                if (reader.GetName(i).Equals(name, StringComparison.OrdinalIgnoreCase))
                    return true;
           
            return false;
        }

        public static PrefixedIDataReader WithPrefix(this IDataReader reader, string? prefix)
        {
            if (reader is PrefixedIDataReader prefixedReader)
                return prefixedReader.WithPrefix(prefix);   

            return new PrefixedIDataReader(reader, prefix);
        }
    }
}
    