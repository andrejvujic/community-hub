using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.Addresses;
using CommunityHub.Application.Domain.Ads;
using CommunityHub.Application.Domain.CommonRooms.RentalRequests;
using CommunityHub.Application.Domain.Events;
using CommunityHub.Application.Domain.Issues;
using CommunityHub.Application.Domain.JobListings;
using CommunityHub.Application.Domain.Neighborhoods.NeighborhoodBudgets;
using CommunityHub.Application.Domain.Neighborhoods.NeighborhoodMeetings;
using CommunityHub.Application.Domain.Notifications;
using CommunityHub.Application.Domain.Notifications.Details;
using CommunityHub.Application.Domain.Users;
using Npgsql;
using System.Data;
using System.Text.Json;

namespace CommunityHub.Application.Database
{
    public class DatabaseConnection : IDisposable
    {
        private readonly IDbConnection _connection;
        private IDbTransaction? transaction;

        private DatabaseConnection()
        {
            _connection = CreateNpgsqlConnection();
            _connection.Open();
        }

        public static DatabaseConnection Create() => new DatabaseConnection();

        private bool IsOpen => _connection.State is ConnectionState.Open;

        public void BeginTransaction()
        {
            if (!IsOpen)
                throw new InvalidOperationException("Connection is not open.");

            if (transaction is not null)
                throw new InvalidOperationException("Transaction already in progress.");

            transaction = _connection.BeginTransaction();
        }

        public void CommitTransaction()
        {
            if (!IsOpen)
                throw new InvalidOperationException("Connection is not open.");

            if (transaction is null)
                throw new InvalidOperationException("No active transaction to commit.");

            transaction.Commit();
            transaction.Dispose();
            transaction = null;
        }

        public void RollbackTranscation()
        {
            if (!IsOpen)
                throw new InvalidOperationException("Connection is not open.");

            if (transaction is null)
                throw new InvalidOperationException("No active transaction to rollback.");

            transaction.Rollback();
            transaction.Dispose();
            transaction = null;
        }

        public void ExecuteReader(DatabaseQuery query, Action<IDataReader> callback)
        {
            if (!IsOpen)
                throw new InvalidOperationException("Connection is not open.");

            using IDbCommand command = query.CreateCommand(_connection);
            using IDataReader reader = command.ExecuteReader();

            while (reader.Read())
                callback.Invoke(reader);
        }
        public int GetRowsAffectedCount(DatabaseQuery query)
        {
            if (!IsOpen)
                throw new InvalidOperationException("Connection is not open.");

            using IDbCommand command = query.CreateCommand(_connection);
            return command.ExecuteNonQuery();
        }

        public object? ExecuteScalar(DatabaseQuery query)
        {
            if (!IsOpen)
                throw new InvalidOperationException("Connection is not open.");

            using IDbCommand command = query.CreateCommand(_connection);
            return command.ExecuteScalar();
        }

        public void Execute(DatabaseQuery query)
        {
            if (!IsOpen)
                throw new InvalidOperationException("Connection is not open.");

            using IDbCommand command = query.CreateCommand(_connection);
            command.ExecuteNonQuery();
        }

        public T ExecuteScalar<T>(DatabaseQuery query, Func<object?, T> convert)
        {
            if (!IsOpen)
                throw new InvalidOperationException("Connection is not open.");

            using IDbCommand command = query.CreateCommand(_connection);
            object? value = command.ExecuteScalar();
            if (value == null) return default!;
            return convert(value);
        }

        private static IDbConnection CreateNpgsqlConnection()
        {
            var connectionString = LoadNpgsqlConnectionString();
            var builder = new NpgsqlDataSourceBuilder(connectionString);

            SetupEnumMapping(builder);

            var dataSource = builder.Build();
            return dataSource.CreateConnection();
        }

        private static string LoadNpgsqlConnectionString()
        {
            var configurationPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            if (!File.Exists(configurationPath))
                throw new FileNotFoundException($"Configuration file not found at: {configurationPath}");

            var json = File.ReadAllText(configurationPath);
            using var document = JsonDocument.Parse(json);

            var connectionString = document.RootElement
                .GetProperty("ConnectionStrings")
                .GetProperty("DefaultConnection")
                .GetString();

            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException($"Connection string 'DefaultConnection' not found in configuration.");

            return connectionString;
        }

        private static void SetupEnumMapping(NpgsqlDataSourceBuilder builder)
        {
            builder.MapEnum<Role>("roles");
            builder.MapEnum<Status>("statuses");
            builder.MapEnum<ParityType>("parity_types");
            builder.MapEnum<BillingType>("billing_types");
            builder.MapEnum<AdCategory>("ad_categories");
            builder.MapEnum<AdStatus>("ad_statuses");
            builder.MapEnum<AdType>("ad_types");
            builder.MapEnum<RentalType>("rental_types");
            builder.MapEnum<MeetingTheme>("meeting_themes");
            builder.MapEnum<MeetingStatus>("meeting_statuses");
            builder.MapEnum<BudgetCategory>("budget_categories");
            builder.MapEnum<RentalRequestStatus>("common_room_rental_statuses");
            builder.MapEnum<EventStatus>("event_statuses");
            builder.MapEnum<NotificationStatus>("notification_statuses");
            builder.MapEnum<NotificationType>("notification_types");
            builder.MapEnum<AdEventType>("ad_event_types");
            builder.MapEnum<IssuePriority>("reported_issue_priorities");
            builder.MapEnum<IssueStatus>("reported_issue_statuses");
        }

        public void Dispose()
        {
            if (transaction is not null)
            {
                transaction.Rollback();
                transaction.Dispose();
                transaction = null;
            }

            _connection.Dispose();
        }
    }
}