using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.NoticeBoard;
using CommunityHub.Application.Domain.CommonRoom;
using CommunityHub.Application.Domain.CommonRoomRentalRequest;
using Npgsql;
using System.Data;
using System.Text.Json;

namespace CommunityHub.Application.Database;

public static class PostgresConnection
{
    private static readonly string ConnectionString = LoadConnectionString();

    public static IDbConnection CreateConnection()
    {
        var builder = new NpgsqlDataSourceBuilder(ConnectionString);
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
        builder.MapEnum<RentalStatus>("common_room_rental_statuses");
        builder.MapEnum<EventStatus>("event_statuses");

        var dataSource = builder.Build();
        return dataSource.OpenConnection();
    }

    private static string LoadConnectionString()
    {
        var configPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

        if (!File.Exists(configPath))
        {
            throw new FileNotFoundException($"Configuration file not found at: {configPath}");
        }

        var json = File.ReadAllText(configPath);
        using var document = JsonDocument.Parse(json);

        var connectionString = document.RootElement
            .GetProperty("ConnectionStrings")
            .GetProperty("DefaultConnection")
            .GetString();

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Connection string 'DefaultConnection' not found in configuration.");
        }

        return connectionString;
    }
}
