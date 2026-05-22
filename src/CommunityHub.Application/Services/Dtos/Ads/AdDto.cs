using CommunityHub.Application.Domain.Ads;
using CommunityHub.Application.Services.Dtos.Users;

namespace CommunityHub.Application.Services.Dtos.Ads
{
    public class AdDto
    {
        public long Id { get; set; }

        public long AuthorId { get; set; }
        public UserDto? Author { get; set; }

        public long BuildingId { get; set; }

        public AdType Type { get; set; }
        public AdCategory Category { get; set; }
        public AdStatus Status { get; set; }

        public string Description { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public AdDto(
            long authorId,
            long buildingId,
            AdType type,
            AdCategory category,
            AdStatus status,
            string description,
            DateTime startDate,
            DateTime endDate,
            UserDto? author = null,
            long id = 0
        )
        {
            Id = id;
            AuthorId = authorId;
            Author = author;
            BuildingId = buildingId;
            Type = type;
            Category = category;
            Status = status;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
        }

        public bool IsOffer => Type is AdType.Offer;
        public bool IsRequest => Type is AdType.Request;

        public bool IsActive => Status is AdStatus.Active;
        public bool IsArchived => Status is AdStatus.Archived;
    }
}