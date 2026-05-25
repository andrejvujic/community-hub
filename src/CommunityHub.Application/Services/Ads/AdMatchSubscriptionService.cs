using CommunityHub.Application.Domain.Ads;
using CommunityHub.Application.Domain.Notifications;
using CommunityHub.Application.Domain.Notifications.Details;
using CommunityHub.Application.Domain.Notifications.RepositoryInterfaces;
using CommunityHub.Application.Domain.RepositoryInterfaces.Ads;
using CommunityHub.Application.MappersDto.Ads;
using CommunityHub.Application.Services;
using CommunityHub.Application.Services.Dtos.Ads;

namespace CommunityHub.Application.Modules.Buildings.Core.Services.Ads
{
    public class AdMatchSubscriptionService : BaseService<AdMatchSubscription, AdMatchSubscriptionDto>
    {
        private readonly IAdMatchSubscriptionRepository _adMatchSubscriptionRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly AdService _adService;

        public AdMatchSubscriptionService(
            IAdRepository adRepository,
            IAdMatchSubscriptionRepository adMatchSubscriptionRepository,
            IAdTimeSlotRepository adTimeSlotRepository,
            INotificationRepository notificationRepository
        ) : base(mapper: new AdMatchSubscriptionMapper())
        {
            _adMatchSubscriptionRepository = adMatchSubscriptionRepository;
            _notificationRepository = notificationRepository;
            _adService = new(adRepository, adTimeSlotRepository);
        }

        public void Create(AdMatchSubscriptionDto subscription)
            => _adMatchSubscriptionRepository.Create(
                    MapToEntity(subscription)
                );

        private List<AdDto> GetSubscribedMatchingAds(AdDto referenceAd)
        {
            var ads = _adService.GetMatchingAds(referenceAd);

            var subscribedAdIds = _adMatchSubscriptionRepository.GetSubscribedAdIds();
            var subscribedAds = ads.Where(
                ad => subscribedAdIds.Contains(ad.Id)
            ).ToList();

            return subscribedAds;
        } 

        public void NotifyAuthorsOfSubscribedMatchingAds(AdDto referenceAd)
        {
            var subscribedAds = GetSubscribedMatchingAds(referenceAd);
            var authorIds = subscribedAds.Select(ad => ad.AuthorId).Distinct();

            var notifications = authorIds.Select(
                authorId => new Notification(
                        recipientId: authorId,
                        title: "Pronađen kompatibilan oglas",
                        message: "Na oglasnoj tabli se pojavio oglas sa odgovarajućom kategorijom i podudarnim datumima.",
                        type: NotificationType.AdEvent,
                        details: new AdEventNotificationDetails(
                            referenceAd.Id,
                            AdEventType.MatchingAdCreated
                        ),
                        sentAt: DateTime.Now
                )
            ).ToList();

            _notificationRepository.CreateMany(notifications);

            _adMatchSubscriptionRepository.DeleteManyByAdId(
                subscribedAds.Select(ad => ad.Id).ToList()
            );
        }
    }
}
