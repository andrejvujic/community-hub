namespace CommunityHub.Application.Domain
{
    public class Neighborhood
    {
        public long Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int Budget { get; private set; }
        public City City { get; private set; }
        public User Coordinator { get; private set; }
        public List<StreetSection> StreetSections { get; private set; } = new List<StreetSection>();
        public List<Image> Images { get; private set; } = new List<Image>();

        public Neighborhood(
            string name,
            string description,
            int budget,
            City city,
            User coordinator,
            long id = 0,
            List<StreetSection>? streetSections = null,
            List<Image>? images = null
        )
        {
            Id = id;
            Name = name;
            Description = description;
            Budget = budget;
            City = city;
            Coordinator = coordinator;
            streetSections = streetSections ?? new List<StreetSection>();
            Images = images ?? new List<Image>();
        }
    }
}
