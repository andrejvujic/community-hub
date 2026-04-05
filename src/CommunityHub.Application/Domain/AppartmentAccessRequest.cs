namespace CommunityHub.Application.Domain
{
    public class AppartmentAccessRequest
    {
        public long Id { get; private set; }
        public User Applicant { get; private set; }
        public Appartment Appartment { get; private set; }
        public string? Comments { get; private set; }
        public Status Status { get; private set; }
        public DateTime CreationDate { get; private set; }

        public AppartmentAccessRequest(
            long id,
            User applicant,
            Appartment appartment,
            string? comments,
            Status status,
            DateTime creationDate
        )
        {
            Id = id;
            Applicant = applicant;
            Appartment = appartment;
            Comments = comments;
            Status = status;
            CreationDate = creationDate;
        }

    }
}
