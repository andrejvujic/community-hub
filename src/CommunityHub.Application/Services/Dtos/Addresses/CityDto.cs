namespace CommunityHub.Application.Services.Dtos.Addresses
{
    public class CityDto
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public CityDto(long id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}