using CommunityHub.Application.Database.Mappers;
using System.Data;

namespace CommunityHub.Application.Database
{
    public class PostgresQuery
    {
        private readonly string _sql;
        private readonly Dictionary<string, object?> _paramaters = new Dictionary<string, object?>();
        private PostgresQuery(string sql)
        {
            _sql = sql;
        }

        public static PostgresQuery Create(string sql) => new PostgresQuery(sql);

        public PostgresQuery AddParameter(string name, object? value)
        {
            _paramaters.Add(name, value);
            return this;
        }

        private IDbCommand CreateCommand(IDbConnection connection)
        {
            IDbCommand command = connection.CreateCommand();
            command.CommandText = _sql;

            foreach (var parameter in _paramaters)
            {
                IDbDataParameter dbParameter = command.CreateParameter();
                dbParameter.ParameterName = parameter.Key;
                dbParameter.Value = parameter.Value;
                command.Parameters.Add(dbParameter);
            }

            return command;
        }

        public List<T> ExecuteReader<T>(Mapper<T> mapper) where T : class
        {
            List<T> entities = new List<T>();
            ExecuteReader(reader => entities.Add(mapper.ToModel(reader)));
            return entities;
        }

        public void ExecuteReader(Action<IDataReader> callback)
        {
            using IDbConnection connection = PostgresConnection.CreateConnection();
            IDbCommand command = CreateCommand(connection);
            IDataReader reader = command.ExecuteReader();;

            while (reader.Read())
                callback.Invoke(reader);
        }

        public List<T> ExecuteVector<T>(Func<IDataReader, T> convert) where T : class
        {

            using IDbConnection connection = PostgresConnection.CreateConnection();
            IDbCommand command = CreateCommand(connection);
            using IDataReader reader = command.ExecuteReader();

            List<T> entities = new List<T>();

            while (reader.Read())
                entities.Add(convert(reader));

            return entities;
        }

        public object? ExecuteScalar()
        {

            using IDbConnection connection = PostgresConnection.CreateConnection();
            IDbCommand command = CreateCommand(connection);
            return command.ExecuteScalar();
        }

        public T ExecuteScalar<T>(Func<object?, T> convert)
        {
            using IDbConnection connection = PostgresConnection.CreateConnection();
            IDbCommand command = CreateCommand(connection);
            object? value = command.ExecuteScalar();
            if (value == null) return default!;
            return convert(value);
        }

        public int GetRowsAffectedCount()
        {
            using IDbConnection connection = PostgresConnection.CreateConnection();
            IDbCommand command = CreateCommand(connection);
            return command.ExecuteNonQuery();
        }
    }
}
