using CommunityHub.Application.Modules.Shared.Core.Domain.Users;

namespace CommunityHub.Application.Modules.Buildings.Core.Domain.Ads
{
    public class Ad
    {
        public long Id { get; private set; }
        public User Author { get; private set; }
        public long BuildingId { get; private set; }

        public AdType Type { get; private set; }
        public AdCategory Category { get; private set; }

        public AdStatus Status { get; private set; }

        public string Description { get; private set; }

        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        public Ad(
            User author,
            long buildingId,
            AdType type,
            AdCategory category,
            AdStatus status,
            string description,
            DateTime startDate,
            DateTime endDate,
            long id = 0
        ) {
            Id = id;

            if (author == null) throw new ArgumentNullException("Author can't be null.");
            Author = author;

            BuildingId = buildingId;

            Type = type;
            Category = category;
            Status = status;

            Description = description;

            if (startDate > endDate) throw new ArgumentNullException("Invalid date range.");

            StartDate = startDate;
            EndDate = endDate;
        }

        public void Archive()
        {
            if (IsArchived) throw new InvalidOperationException("Ad is already archived.");
            Status = AdStatus.Archived;
        }

        public bool IsOffer => Type is AdType.Offer;
        public bool IsRequest => Type is AdType.Request;

        public bool IsActive => Status is AdStatus.Active;
        public bool IsArchived => Status is AdStatus.Archived;

        public bool Matches(Ad other)
        {
            if (other == null) return false;

            if (Id == other.Id) return false;
            if (Author.Id == other.Author.Id) return false;
            if (BuildingId != other.BuildingId) return false;

            if (Type == other.Type) return false;
            if (Category != other.Category) return false;
            if (!other.IsActive) return false;

            if (StartDate > other.EndDate) return false;
            if (other.StartDate > EndDate) return false;

            return true;
        }


        public Ad CopyWith(
            long? id = null,
            User? author = null,
            long? buildingId = null,
            AdType? type = null,
            AdCategory? category = null,
            AdStatus? status = null,
            string? description = null,
            DateTime? startDate = null,
            DateTime? endDate = null
        )
        {
            return new Ad(
                author ?? Author,
                buildingId ?? BuildingId,
                type ?? Type,
                category ?? Category,
                status ?? Status,
                description ?? Description,
                startDate ?? StartDate,
                endDate ?? EndDate,
                id: id ?? Id
            );
        }
    }
}
