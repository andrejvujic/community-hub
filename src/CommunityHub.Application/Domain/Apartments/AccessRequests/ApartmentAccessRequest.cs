using CommunityHub.Application.Modules.Shared.Core.Domain.Users;

namespace CommunityHub.Application.Domain.Apartments.AccessRequests
{
    public class ApartmentAccessRequest
    {
        public long Id { get; private set; }

        public long ApplicantId { get; private set; }
        public User? Applicant { get; private set; }

        public long ApartmentId { get; private set; }
        public Apartment? Apartment { get; private set; }

        public string? Comment { get; private set; }
        public Status Status { get; private set; }
        public DateTime CreationDate { get; private set; }

        public bool IsPending => Status == Status.Waiting;

        public bool HasComment => !string.IsNullOrWhiteSpace(Comment);

        public ApartmentAccessRequest(
            long applicantId,
            long apartmentId,
            Status status,
            DateTime creationDate,
            long id = 0,
            string? comment = null,
            User? applicant = null,
            Apartment? apartment = null
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
            long? applicantId = null,
            User? applicant = null,
            long? apartmentId = null,
            Apartment? apartment = null,
            Status? status = null,
            DateTime? creationDate = null,
            string? comment = null
        )
        {
            return new ApartmentAccessRequest(
                applicantId: applicantId ?? ApplicantId,
                applicant: applicant ?? Applicant,
                apartmentId: apartmentId ?? ApartmentId,
                apartment: apartment ?? Apartment,
                status: status ?? Status,
                creationDate: creationDate ?? CreationDate,
                id: id ?? Id,
                comment: comment ?? Comment
            );
        }
    }
}