using CommunityHub.Application.Domain.Ads;
using CommunityHub.Application.Services.Dtos.Ads;

namespace CommunityHub.Application.MappersDto.Ads
{
    public class AdMatchSubscriptionMapper : IMapper<AdMatchSubscription, AdMatchSubscriptionDto>
    {
        private readonly AdMapper _adMapper = new();
        public AdMatchSubscription MapToEntity(AdMatchSubscriptionDto dto)
            => new AdMatchSubscription(
                subscribedAd: dto.SubscribedAd is null ? null: _adMapper.MapToEntity(dto.SubscribedAd),
                subscribedAdId: dto.SubscribedAdId,
                id: dto.Id
            );

        public AdMatchSubscriptionDto MapToDto(AdMatchSubscription domain)
            => new AdMatchSubscriptionDto(
                subscribedAdId: domain.SubscribedAdId,  
                subscribedAd: domain.SubscribedAd is null ? null: _adMapper.MapToDto(domain.SubscribedAd),
                id: domain.Id
            );
    }
}
