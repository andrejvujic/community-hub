namespace CommunityHub.Application.Domain
{
    public class ApartmentAccessRequest
    {
        public long Id { get; private set; }
        public User Applicant { get; private set; }
        public Apartment Apartment { get; private set; }
        public string? Comment { get; private set; }
        public Status Status { get; private set; }
        public DateTime CreationDate { get; private set; }

        public ApartmentAccessRequest(
            long id,
            User applicant,
            Apartment apartment,
            string? comment,
            Status status,
            DateTime creationDate
        )
        {
            Id = id;
            Applicant = applicant;
            Apartment = apartment;
            Comment = comment;
            Status = status;
            CreationDate = creationDate;
        }

    }
}
