
namespace CommunityHub.Application.Domain
{
    public class ApartmentMembership(User resident, Apartment apartment, DateTime acceptanceDate, long id = 0)
    {
        public long Id { get; private set; } = id;
        public User Resident { get; private set; } = resident;
        public Apartment Apartment { get; private set; } = apartment;
        public DateTime AcceptanceDate { get; private set; } = acceptanceDate;

        public ApartmentMembership CopyWith(
            long? id = null,
            User? resident = null,
            Apartment? apartment = null,
            DateTime? acceptanceDate = null
        )
        {
            return new ApartmentMembership(
                resident ?? Resident,
                apartment ?? Apartment,
                acceptanceDate ?? AcceptanceDate,
                id ?? Id
            );
        }
    }
}
