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
                ad.Id,
                ad.AuthorId,
                ad.Author != null ? _userMapper.MapToDto(ad.Author) : null,
                ad.BuildingId,
                ad.Type,
                ad.Category,
                ad.Status,
                ad.Description,
                ad.StartDate,
                ad.EndDate
            );
        }
        public Ad MapToDomain(AdDto dto)
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
                dto.Author != null ? _userMapper.MapToDomain(dto.Author) : null,
                dto.Id
            );
        }
    }
}