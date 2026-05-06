using CommunityHub.Application.Modules.Shared.Core.Domain.Users;

namespace CommunityHub.Application.Domain.Apartments.AccessRequests
{
    public class ApartmentAccessRequest
    {
        public long Id { get; private set; }
        public User Applicant { get; private set; }
        public Apartment Apartment { get; private set; }
        public string? Comment { get; private set; }
        public Status Status { get; private set; }
        public DateTime CreationDate { get; private set; }
        public bool IsCancellable => Status == Status.Waiting;
        public bool HasComment => !string.IsNullOrWhiteSpace(Comment);

        public ApartmentAccessRequest(
            User applicant,
            Apartment apartment,
            Status status,
            DateTime creationDate,
            long id = 0,
            string? comment = null
        )
        {
            Id = id;
            Applicant = applicant;
            Apartment = apartment;
            Comment = comment;
            Status = status;
            CreationDate = creationDate;
        }
        public ApartmentAccessRequest CopyWith(
            long? id = null,
            User? applicant = null,
            Apartment? apartment = null,
            Status? status = null,
            DateTime? creationDate = null,
            string? comment = null
        )
        {
            return new ApartmentAccessRequest(
                applicant ?? Applicant,
                apartment ?? Apartment,
                status ?? Status,
                creationDate ?? CreationDate,
                id ?? Id,
                comment ?? Comment
            );
        }
    }
}