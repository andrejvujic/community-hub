using CommunityHub.Application.Domain;
using Npgsql;
using System.Data;
using System.Text.Json;
using CommunityHub.Application.Domain.CommonRooms.RentalRequests;
using CommunityHub.Application.Domain.Issues;
using CommunityHub.Application.Domain.Ads;
using CommunityHub.Application.Domain.Notifications;
using CommunityHub.Application.Domain.Notifications.Details;
using CommunityHub.Application.Domain.Users;
using CommunityHub.Application.Domain.Neighborhoods.NeighborhoodMeetings;
using CommunityHub.Application.Domain.Neighborhoods.NeighborhoodBudgets;
using CommunityHub.Application.Domain.Events;
using CommunityHub.Application.Domain.Services;
using CommunityHub.Application.Domain.Addresses;
using CommunityHub.Application.Domain.Neighborhoods;

namespace CommunityHub.Application.Database;

[Obsolete("Use DatabaseConnection instead.")]
public static class PostgresConnection
{
    private static readonly string ConnectionString = LoadConnectionString();

    public static IDbConnection CreateConnection()
    {
        var builder = new NpgsqlDataSourceBuilder(ConnectionString);
        AddEnumMapping(builder);

        var dataSource = builder.Build();
        return dataSource.OpenConnection();
    }

    private static void AddEnumMapping(NpgsqlDataSourceBuilder builder)
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
        builder.MapEnum<NeighborhoodAccessRequestStatus>("neighborhood_access_request_statuses");
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
