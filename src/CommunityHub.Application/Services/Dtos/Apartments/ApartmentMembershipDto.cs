using CommunityHub.Application.Services.Dtos.Users;

namespace CommunityHub.Application.Services.Dtos.Apartments
{
    public class ApartmentMembershipDto
    {
        public long Id { get; set; }
        public UserDto Resident { get; set; }
        public ApartmentDto Apartment { get; set; }
        public DateTime AcceptanceDate { get; set; }

        public ApartmentMembershipDto(long id, UserDto resident, ApartmentDto apartment, DateTime acceptanceDate)
        {
            Id = id;
            Resident = resident;
            Apartment = apartment;
            AcceptanceDate = acceptanceDate;
        }
    }
}