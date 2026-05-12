namespace CommunityHub.Application.Services.Dtos.Addresses
{
    public class StreetDto
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public StreetDto(long id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}