namespace CommunityHub.Application.Domain
{
    public class ApartmentAccessRequest
    {
        public long Id { get; private set; }
        public User Applicant { get; private set; }
        public Apartment Apartment { get; private set; }
        public string? Comments { get; private set; }
        public Status Status { get; private set; }
        public DateTime CreationDate { get; private set; }

        public ApartmentAccessRequest(
            long id,
            User applicant,
            Apartment apartment,
            string? comments,
            Status status,
            DateTime creationDate
        )
        {
            Id = id;
            Applicant = applicant;
            Apartment = apartment;
            Comments = comments;
            Status = status;
            CreationDate = creationDate;
        }

    }
}
