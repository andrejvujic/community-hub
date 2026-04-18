namespace CommunityHub.Application.Domain
{
    public class CoverageOffer
    {
        public long Id { get; private set; }
        public LeaveRequest LeaveRequest { get; private set; }
        public BookedJob JobToCover { get; private set; }
        public ServiceProvider CoveringProvider { get; private set; }

        public Status Status { get; private set; }

        public bool IsPending => Status == Status.Waiting;

        public CoverageOffer(
            LeaveRequest leaveRequest,
            BookedJob jobToCover,
            ServiceProvider coveringProvider,
            long id = 0,
            Status status = Status.Waiting
        )
        {
            if (coveringProvider.Id == jobToCover.Listing.Provider.Id)
                throw new ArgumentException(
                    "Serviser zamene ne sme biti isti kao originalni pružalac usluge.",
                    nameof(coveringProvider));

            Id = id;
            LeaveRequest = leaveRequest;
            JobToCover = jobToCover;
            CoveringProvider = coveringProvider;
            Status = status;
        }

        public CoverageOffer CopyWith(
            long? id = null,
            LeaveRequest? leaveRequest = null,
            BookedJob? jobToCover = null,
            ServiceProvider? coveringProvider = null,
            Status? status = null
        )
        {
            return new CoverageOffer(
                leaveRequest ?? LeaveRequest,
                jobToCover ?? JobToCover,
                coveringProvider ?? CoveringProvider,
                id ?? Id,
                status ?? Status
            );
        }
    }
}