namespace CommunityHub.Application.Domain
{
    public class LeaveRequest
    {
        public long Id { get; private set; }
        public ServiceProvider Provider { get; private set; }
        public DateOnly StartDate { get; private set; }
        public DateOnly EndDate { get; private set; }
        public LeaveRequestStatus Status { get; private set; }
        public bool IsEmergency { get; private set; }
        public List<BookedJob> AffectedJobs { get; private set; } = new List<BookedJob>();

        public LeaveRequest(
            ServiceProvider provider,
            DateOnly startDate,
            DateOnly endDate,
            LeaveRequestStatus status,
            bool isEmergency,
            long id = 0,
            List<BookedJob>? affectedJobs = null
        )
        {
            Id = id;
            Provider = provider;
            StartDate = startDate;
            EndDate = endDate;
            Status = status;
            IsEmergency = isEmergency;
            AffectedJobs = affectedJobs ?? new List<BookedJob>();
        }

        public LeaveRequest CopyWith(
            long? id = null,
            ServiceProvider? provider = null,
            DateOnly? startDate = null,
            DateOnly? endDate = null,
            LeaveRequestStatus? status = null,
            bool? isEmergency = null,
            List<BookedJob>? affectedJobs = null
        )
        {
            return new LeaveRequest(
                provider ?? Provider,
                startDate ?? StartDate,
                endDate ?? EndDate,
                status ?? Status,
                isEmergency ?? IsEmergency,
                id ?? Id,
                affectedJobs ?? AffectedJobs
            );
        }
    }
}