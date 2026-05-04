using CommunityHub.Application.Modules.Buildings.API.Public.Ads;
using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads;
using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.RepositoryInterfaces;
using CommunityHub.Application.Modules.Shared.Core.Domain.Notifications;
using CommunityHub.Application.Modules.Shared.Core.Domain.Notifications.Details;
using CommunityHub.Application.Modules.Shared.Core.Domain.Notifications.RepositoryInterfaces;

namespace CommunityHub.Application.Modules.Buildings.Core.Services.Ads
{
    public class AdMatchSubscriptionService: IAdMatchSubscriptionService
    {
        private readonly IAdMatchSubscriptionRepository _adMatchSubscriptionRepository;
        private readonly IAdService _adService;
        private readonly INotificationRepository _notificationRepository;

        public AdMatchSubscriptionService(
            IAdMatchSubscriptionRepository adMatchSubscriptionRepository,
            IAdService adService,
            INotificationRepository notificationRepository)
        {
            _adMatchSubscriptionRepository = adMatchSubscriptionRepository;
            _adService = adService;
            _notificationRepository = notificationRepository;
        }

        public bool IsSubscribed(long adId) => _adMatchSubscriptionRepository.IsSubscribed(adId);

        public void NotifyAuthorsOfSubscribedMatchingAds(Ad referenceAd)
        {
            var ads = _adService.GetMatchingAds(referenceAd);
            var authorIds = ads.Select(ad => ad.Author.Id).Distinct();

            foreach (var authorId in authorIds)
                _notificationRepository.Create(
                    Notification.Create(
                        recipientId: authorId,
                        title: "Pronađen kompatibilan oglas",
                        message: "Na oglasnoj tabli se pojavio oglas sa odgovarajućom kategorijom i podudarnim datumima.",
                        type: NotificationType.AdEvent,
                        details: new AdEventNotificationDetails(
                            referenceAd.Id,
                            AdEventType.MatchingAdCreated
                        )
                    )
                );

            foreach (var ad in ads)
                _adMatchSubscriptionRepository.DeleteByAdId(ad.Id);
        }
    }
}
