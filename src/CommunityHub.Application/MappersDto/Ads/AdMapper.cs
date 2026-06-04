using CommunityHub.Application.Domain.Ads;
using CommunityHub.Application.Services.Dtos.Ads;
using CommunityHub.Application.MappersDto.Users;
namespace CommunityHub.Application.MappersDto.Ads
{
    public class AdMapper : IMapper<Ad, AdDto>
    {
        private readonly UserMapper _userMapper = new();
        public AdDto MapToDto(Ad ad)
        {
            if (ad == null) throw new ArgumentNullException(nameof(ad));

            return new AdDto(
                id: ad.Id,
                authorId: ad.AuthorId,
                author: ad.Author != null ? _userMapper.MapToDto(ad.Author) : null,
                buildingId: ad.BuildingId,
                type: ad.Type,
                category: ad.Category,
                status: ad.Status,
                description: ad.Description,
                startDate: ad.StartDate,
                endDate: ad.EndDate,
                isSubscribedToMatchingAds: ad.IsSubscribedToMatchingAds
            );
        }
        public Ad MapToEntity(AdDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new Ad(
                dto.AuthorId,
                dto.BuildingId,
                dto.Type,
                dto.Category,
                dto.Status,
                dto.Description,
                dto.StartDate,
                dto.EndDate,
                dto.IsSubscribedToMatchingAds,
                dto.Author is not null ? _userMapper.MapToEntity(dto.Author) : null,
                dto.Id
            );
        }
    }
}
