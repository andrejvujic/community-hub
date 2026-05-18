using System.Data;

namespace CommunityHub.Application.Database
{
    public class DatabaseQuery
    {
        private readonly string _sql;
        private readonly Dictionary<string, object?> _parameters = new();
        private DatabaseQuery(string sql)
        {
            _sql = sql;
        }

        public static DatabaseQuery Create(string sql) => new(sql);

        public DatabaseQuery AddParameter(string name, object? value)
        {
            _parameters.Add(name, value);
            return this;
        }

        public DatabaseQuery ExtendParameters(Dictionary<string, object?> newParameters)
        {
            foreach (var parameter in newParameters)
                _parameters.Add(parameter.Key, parameter.Value);
            return this;
        }

        public IDbCommand CreateCommand(IDbConnection connection)
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
    }
}
