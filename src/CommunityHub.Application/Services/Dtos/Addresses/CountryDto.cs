namespace CommunityHub.Application.Services.Dtos.Addresses
{
    public class CountryDto
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public CountryDto(long id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}