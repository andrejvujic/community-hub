using CommunityHub.Application.Domain.Users;

namespace CommunityHub.Application.Domain.ServiceBookings
{
    public class LeaveRequest
    {
        private static readonly TimeSpan EmergencyThreshold = TimeSpan.FromHours(48);

        public long Id { get; private set; }
        public ServiceProvider Provider { get; private set; }
        public DateOnly StartDate { get; private set; }
        public DateOnly EndDate { get; private set; }
        public Status Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public List<long> AffectedJobIds { get; private set; } = new List<long>();

        public bool IsEmergency() =>
            StartDate.ToDateTime(TimeOnly.MinValue) - CreatedAt < EmergencyThreshold;

        public LeaveRequest(
            ServiceProvider provider,
            DateOnly startDate,
            DateOnly endDate,
            Status status,
            DateTime createdAt,
            long id = 0,
            List<long>? affectedJobIds = null
        )
        {
            if (endDate < startDate)
                throw new ArgumentException(
                    $"Datum završetka ({endDate}) ne sme biti pre datuma početka ({startDate}).",
                    nameof(endDate));

            Id = id;
            Provider = provider;
            StartDate = startDate;
            EndDate = endDate;
            Status = status;
            CreatedAt = createdAt;
            AffectedJobIds = affectedJobIds ?? new List<long>();
        }

        public LeaveRequest CopyWith(
            long? id = null,
            ServiceProvider? provider = null,
            DateOnly? startDate = null,
            DateOnly? endDate = null,
            Status? status = null,
            DateTime? createdAt = null,
            List<long>? affectedJobIds = null
        )
        {
            return new LeaveRequest(
                provider ?? Provider,
                startDate ?? StartDate,
                endDate ?? EndDate,
                status ?? Status,
                createdAt ?? CreatedAt,
                id ?? Id,
                affectedJobIds ?? AffectedJobIds
            );
        }
    }
}