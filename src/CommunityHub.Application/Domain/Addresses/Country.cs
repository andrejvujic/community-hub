namespace CommunityHub.Application.Domain.Addresses
{
    public class Country
    {
        public long Id { get; private set; }
        public string Name { get; private set; }

        public Country(long id, string name)
        {
            Id = id;
            Name = name;
        }
        public Country CopyWith(
            long? id = null,
            string? name = null
        )
        {
            return new Country(
                id ?? Id,
                name ?? Name
            );
        }
        public bool Equals(Country? other)
        {
            if (other is null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return Name == other.Name;
        }
        public override bool Equals(object? other)
        {
            if (other == null || other is not Country) return false;
            return Equals(other as Country);
        }

        public override int GetHashCode()
        {
            return Name?.GetHashCode() ?? 0;
        }
        public static bool operator ==(Country? left, Country? right)
        {
            if (left is null)
                return right is null;

            return left.Equals(right);
        }
        public static bool operator !=(Country? left, Country? right)
        {
            return !(left == right);
        }
    }
}