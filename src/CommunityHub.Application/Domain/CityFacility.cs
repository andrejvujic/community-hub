namespace CommunityHub.Application.Domain
{
    public class CityFacility
    {
        public long Id { get; private set; }
        public City City { get; private set; }
        public string Name { get; private set; }

        public CityFacility(
            City city,
            string name,
            long id = 0
        )
        {
            Id = id;
            City = city;
            Name = name;
        }
    }
}