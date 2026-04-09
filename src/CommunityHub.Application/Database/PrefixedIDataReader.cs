using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace CommunityHub.Application.Database
{
    public class PrefixedIDataReader : IDataReader
    {
        private readonly IDataReader _reader;
        private readonly string? _prefix;

        // IDataReader wrapper that applies a prefix to column names when accessing data.
        public PrefixedIDataReader(IDataReader reader, string? prefix = null) {
            _reader = reader;
            _prefix = prefix;
        }
            
        public PrefixedIDataReader WithPrefix(string? additionalPrefix)
        {
            // Allows chaining of prefixes by appending an additional
            // prefix to the existing one.
            if (additionalPrefix == null) return this;
            if (_prefix == null) return new PrefixedIDataReader(_reader, additionalPrefix);

            return new PrefixedIDataReader(_reader, $"{_prefix}_{additionalPrefix}");
        }

        private string ApplyPrefix(string name) => _prefix == null ? name : $"{_prefix}_{name}";

        public int GetOrdinal(string name) => _reader.GetOrdinal(ApplyPrefix(name));

        public bool HasColumn(string name) => _reader.HasColumn(ApplyPrefix(name));

        public IDataReader GetData(int i) => new PrefixedIDataReader(_reader.GetData(i), _prefix);

        public object this[string name] => _reader[ApplyPrefix(name)];

        // The methods below are wrappers around the IDataReader methods
        // and do not change their logic.

        public object this[int i] => _reader[i];

        public int Depth => _reader.Depth;

        public bool IsClosed => _reader.IsClosed;

        public int RecordsAffected => _reader.RecordsAffected;

        public int FieldCount => _reader.FieldCount;

        public void Close() => _reader.Close();

        public void Dispose() => _reader.Dispose();

        public bool GetBoolean(int i) => _reader.GetBoolean(i);

        public byte GetByte(int i) => _reader.GetByte(i);

        public long GetBytes(int i, long fieldOffset, byte[]? buffer, int bufferOffset, int length)
                => _reader.GetBytes(i, fieldOffset, buffer, bufferOffset, length);

        public char GetChar(int i) => _reader.GetChar(i);

        public long GetChars(int i, long fieldoffset, char[]? buffer, int bufferOffset, int length)
                => _reader.GetChars(i, fieldoffset, buffer, bufferOffset, length);

        public string GetDataTypeName(int i) => _reader.GetDataTypeName(i);

        public DateTime GetDateTime(int i) => _reader.GetDateTime(i);

        public decimal GetDecimal(int i) => _reader.GetDecimal(i);

        public double GetDouble(int i) => _reader.GetDouble(i);

        [return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)]
        public Type GetFieldType(int i) => _reader.GetFieldType(i);

        public float GetFloat(int i) => _reader.GetFloat(i);

        public Guid GetGuid(int i) => _reader.GetGuid(i);

        public short GetInt16(int i) => _reader.GetInt16(i);

        public int GetInt32(int i) => _reader.GetInt32(i);

        public long GetInt64(int i) => _reader.GetInt64(i);

        public string GetName(int i) => _reader.GetName(i);

        public DataTable? GetSchemaTable() => _reader.GetSchemaTable();

        public string GetString(int i) => _reader.GetString(i);

        public object GetValue(int i) => _reader.GetValue(i);

        public int GetValues(object[] values) => _reader.GetValues(values);

        public bool IsDBNull(int i) => _reader.IsDBNull(i);

        public bool NextResult() => _reader.NextResult();

        public bool Read() => _reader.Read();
    }
}
