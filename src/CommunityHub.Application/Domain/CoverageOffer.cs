namespace CommunityHub.Application.Domain
{
    public class CoverageOffer
    {
        public long Id { get; private set; }
        public LeaveRequest LeaveRequest { get; private set; }
        public BookedJob JobToCover { get; private set; }
        public ServiceProvider CoveringProvider { get; private set; }

        public bool? IsAccepted { get; private set; }

        public bool IsPending => IsAccepted == null;

        public CoverageOffer(
            LeaveRequest leaveRequest,
            BookedJob jobToCover,
            ServiceProvider coveringProvider,
            long id = 0,
            bool? isAccepted = null
        )
        {
            Id = id;
            LeaveRequest = leaveRequest;
            JobToCover = jobToCover;
            CoveringProvider = coveringProvider;
            IsAccepted = isAccepted;
        }

        public CoverageOffer CopyWith(
            long? id = null,
            LeaveRequest? leaveRequest = null,
            BookedJob? jobToCover = null,
            ServiceProvider? coveringProvider = null,
            bool? isAccepted = null,
            bool clearIsAccepted = false
        )
        {
            return new CoverageOffer(
                leaveRequest ?? LeaveRequest,
                jobToCover ?? JobToCover,
                coveringProvider ?? CoveringProvider,
                id ?? Id,
                clearIsAccepted ? null : (isAccepted ?? IsAccepted)
            );
        }
    }
}