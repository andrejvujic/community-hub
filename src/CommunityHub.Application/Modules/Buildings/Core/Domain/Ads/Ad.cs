using CommunityHub.Application.Modules.Shared.Core.Domain.Users;

namespace CommunityHub.Application.Modules.Buildings.Core.Domain.Ads
{
    public class Ad
    {
        public long Id { get; }

        public long AuthorId { get; }
        public User? Author { get; }

        public long BuildingId { get; }

        public AdType Type { get; }
        public AdCategory Category { get; }
        public AdStatus Status { get; private set; }

        public string Description { get; }

        public DateTime StartDate { get; }
        public DateTime EndDate { get; }

        public Ad(
            long authorId,
            long buildingId,
            AdType type,
            AdCategory category,
            AdStatus status,
            string description,
            DateTime startDate,
            DateTime endDate,
            User? author = null,
            long id = 0
        ) {
            Id = id;

            AuthorId = authorId;
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

        public bool HasExpired => EndDate.Date < DateTime.Today;

        public bool Matches(Ad other)
        {
            if (other == null) return false;

            if (Id == other.Id) return false;
            if (AuthorId == other.AuthorId) return false;
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
            long? authorId = null,
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
                author: author ?? Author,
                authorId: authorId ?? AuthorId, 
                buildingId: buildingId ?? BuildingId,
                type: type ?? Type,
                category: category ?? Category,
                status: status ?? Status,
                description: description ?? Description,
                startDate: startDate ?? StartDate,
                endDate: endDate ?? EndDate,
                id: id ?? Id
            );
        }
    }
}
