using CommunityHub.Application.Services.Dtos.Addresses;

namespace CommunityHub.Application.Services.Dtos.Users
{
    public class ServiceProviderDto
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public CityDto City { get; set; }
        public CountryDto Country { get; set; }

        public ServiceProviderDto(
            long id,
            string username,
            string password,
            string name,
            string surname,
            CityDto city,
            CountryDto country)
        {
            Id = id;
            Username = username;
            Password = password;
            Name = name;
            Surname = surname;
            City = city;
            Country = country;
        }
    }
}
