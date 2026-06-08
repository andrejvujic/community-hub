using CommunityHub.Application.Domain.Users;
using CommunityHub.Application.Services.Dtos.Addresses;
using CommunityHub.Application.Services.Dtos.Neighborhoods;

namespace CommunityHub.Application.Services.Dtos.Users
{
    public class CitizenDto
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public Role Role { get; set; }
        public AddressDto Address { get; set; }
        public NeighborhoodMembershipDto? Membership { get; set; }

        public CitizenDto(
            long id,
            string username,
            string password,
            string name,
            string surname,
            Role role,
            AddressDto address,
            NeighborhoodMembershipDto? membership = null
        )
        {
            Id = id;
            Username = username;
            Password = password;
            Name = name;
            Surname = surname;
            Role = role;
            Address = address;
            Membership = membership;
        }

        public bool CanWriteCoordinatorReview(int removedReviewCount, bool hasRecentReviewThisMonth)
        {
            return removedReviewCount < 3 && !hasRecentReviewThisMonth;
        }
    }
}
