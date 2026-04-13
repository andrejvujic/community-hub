namespace CommunityHub.Application.Domain
{
    public class City : IEquatable<City>
    {
        public long Id { get; private set; }
        public string Name { get; private set; }
        public City(long id, string name)
        {
            Id = id;
            Name = name;
        }
        public City CopyWith(
            long? id = null,
            string? name = null
        )
        {
            return new City(
                id ?? Id,
                name ?? Name
            );
        }
        public bool Equals(City? other)
        {
            if (other is null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return Name == other.Name;
        }

        public override bool Equals(object? other)
        {
            if (other == null ||  other is not City) return false;
            return Equals(other as City);
        }

        public override int GetHashCode()
        {
            return Name?.GetHashCode() ?? 0;
        }

        public static bool operator ==(City? left, City? right)
        {
            if (left is null)
                return right is null;

            return left.Equals(right);
        }

        public static bool operator !=(City? left, City? right)
        {
            return !(left == right);
        }
    }
}

