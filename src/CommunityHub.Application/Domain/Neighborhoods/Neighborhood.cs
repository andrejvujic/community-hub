using CommunityHub.Application.Domain.Addresses;
using CommunityHub.Application.Domain.Users;

namespace CommunityHub.Application.Domain.Neighborhoods
{
    public class Neighborhood
    {
        public long Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int Budget { get; private set; }
        public City City { get; private set; }
        public Country Country { get; private set; }
        public User Coordinator { get; private set; }
        public List<StreetSection> StreetSections { get; private set; } = new List<StreetSection>();
        public List<Image> Images { get; private set; } = new List<Image>();

        public Neighborhood(
            string name,
            string description,
            int budget,
            City city,
            Country country,
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
            Country = country;
            Coordinator = coordinator;
            StreetSections = streetSections ?? new List<StreetSection>();
            Images = images ?? new List<Image>();
        }

        public Neighborhood CopyWith(
            long? id = null,
            string? name = null,
            string? description = null,
            int? budget = null,
            City? city = null,
            Country? country = null,
            User? coordinator = null,
            List<StreetSection>? streetSections = null,
            List<Image>? images = null
        )
        {
            return new Neighborhood(
                name ?? Name,
                description ?? Description,
                budget ?? Budget,
                city ?? City,
                country ?? Country,
                coordinator ?? Coordinator,
                id ?? Id,
                streetSections ?? StreetSections,
                images ?? Images
            );
        }

        public bool Contains(Address? address)
        {
            if (address == null || address.Street == null) return false;
            if (!int.TryParse(address.StreetNumber, out int streetNumber)) return false;

            foreach (var s in StreetSections)
            {
                if (s.Street == null || s.Street.Id != address.Street.Id) continue;
                if (streetNumber < s.StartNumber || streetNumber > s.EndNumber) continue;

                var side = s.Side;
                if (side == ParityType.Both) return true;
                if (side == ParityType.Even && (streetNumber % 2) == 0) return true;
                if (side == ParityType.Odd && (streetNumber % 2) != 0) return true;
            }

            return false;
        }
    }
}
