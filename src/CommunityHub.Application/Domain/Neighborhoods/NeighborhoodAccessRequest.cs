using CommunityHub.Application.Domain.Users;

namespace CommunityHub.Application.Domain.Neighborhoods
{
    public class NeighborhoodAccessRequest
    {
        public Citizen Citizen { get; private set; }
        public Neighborhood Neighborhood { get; private set; }
        public NeighborhoodAccessRequestStatus Status { get; private set; }
        public DateTime CreationDate { get; private set; }
        public string? Comment { get; private set; }

        public NeighborhoodAccessRequest(
            Citizen citizen,
            Neighborhood neighborhood,
            NeighborhoodAccessRequestStatus status,
            DateTime creationDate,
            string? comment = null
        )
        {
            Citizen = citizen;
            Neighborhood = neighborhood;
            Status = status;
            CreationDate = creationDate;
            Comment = comment;
        }

        public NeighborhoodAccessRequest CopyWith(
            Citizen? citizen = null,
            Neighborhood? neighborhood = null,
            NeighborhoodAccessRequestStatus? status = null,
            DateTime? creationDate = null,
            string? comment = null
        )
        {
            return new NeighborhoodAccessRequest(
                citizen ?? Citizen,
                neighborhood ?? Neighborhood,
                status ?? Status,
                creationDate ?? CreationDate,
                comment ?? Comment
            );
        }

        public NeighborhoodAccessRequest Accept()
        {
            return CopyWith(status: NeighborhoodAccessRequestStatus.Accepted);
        }

        public NeighborhoodAccessRequest Reject(string comment)
        {
            return CopyWith(status: NeighborhoodAccessRequestStatus.Rejected, comment: comment);
        }
    }
}
