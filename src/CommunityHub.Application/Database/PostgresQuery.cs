using System.Data;

namespace CommunityHub.Application.Database
{
    [Obsolete("Use DatabaseQuery instead.")]
    public class PostgresQuery
    {
        private readonly string _sql;
        private readonly Dictionary<string, object?> _parameters = [];
        private PostgresQuery(string sql)
        {
            _sql = sql;
        }

        public static PostgresQuery Create(string sql) => new(sql);

        public PostgresQuery AddParameter(string name, object? value)
        {
            _parameters.Add(name, value);
            return this;
        }

        public PostgresQuery ExtendParameters(Dictionary<string, object?> newParameters)
        {
            foreach (var parameter in newParameters)
                _parameters.Add(parameter.Key, parameter.Value);
            return this;
        }

        private IDbCommand CreateCommand(IDbConnection connection)
        {
            IDbCommand command = connection.CreateCommand();
            command.CommandText = _sql;

            foreach (var parameter in _parameters)
            {
                IDbDataParameter dbParameter = command.CreateParameter();
                dbParameter.ParameterName = parameter.Key;
                dbParameter.Value = parameter.Value ?? DBNull.Value;
                command.Parameters.Add(dbParameter);
            }

            return command;
        }

        public void ExecuteReader(Action<IDataReader> callback)
        {
            using IDbConnection connection = PostgresConnection.CreateConnection();
            using IDbCommand command = CreateCommand(connection);
            using IDataReader reader = command.ExecuteReader();

            while (reader.Read())
                callback.Invoke(reader);
        }

        public object? ExecuteScalar()
        {
            using IDbConnection connection = PostgresConnection.CreateConnection();
            using IDbCommand command = CreateCommand(connection);
            return command.ExecuteScalar();
        }

        public void Execute()
        {
            using IDbConnection connection = PostgresConnection.CreateConnection();
            using IDbCommand command = CreateCommand(connection);
            command.ExecuteNonQuery();
        }

        public T ExecuteScalar<T>(Func<object?, T> convert)
        {
            using IDbConnection connection = PostgresConnection.CreateConnection();
            using IDbCommand command = CreateCommand(connection);
            object? value = command.ExecuteScalar();
            if (value == null) return default!;
            return convert(value);
        }

        public int GetRowsAffectedCount()
        {
            using IDbConnection connection = PostgresConnection.CreateConnection();
            using IDbCommand command = CreateCommand(connection);
            return command.ExecuteNonQuery();
        }
    }
}
