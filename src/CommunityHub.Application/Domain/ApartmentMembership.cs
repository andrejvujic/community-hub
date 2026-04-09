
namespace CommunityHub.Application.Domain
{
    public class ApartmentMembership
    {
        public long Id { get; private set; }
        public User Resident {  get; private set; }
        public Apartment Apartment { get; private set; }
        public DateTime AcceptanceDate { get; private set; }
        public ApartmentMembership(long id, User resident, Apartment apartment, DateTime acceptanceDate)
        {
            Id = id;
            Resident = resident;
            Apartment = apartment;
            AcceptanceDate = acceptanceDate;
        }

        public ApartmentMembership CopyWith(
            long? id = null,
            User? resident = null,
            Apartment? apartment = null,
            DateTime? acceptanceDate = null
        )
        {
            return new ApartmentMembership(
                id ?? Id,
                resident ?? Resident,
                apartment ?? Apartment,
                acceptanceDate ?? AcceptanceDate
            );
        }
    }
}
