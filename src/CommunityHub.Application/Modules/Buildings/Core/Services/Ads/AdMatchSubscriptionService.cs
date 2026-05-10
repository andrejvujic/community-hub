using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads;
using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.RepositoryInterfaces;
using CommunityHub.Application.Modules.Shared.Core.Domain.Notifications;
using CommunityHub.Application.Modules.Shared.Core.Domain.Notifications.Details;
using CommunityHub.Application.Modules.Shared.Core.Domain.Notifications.RepositoryInterfaces;

namespace CommunityHub.Application.Modules.Buildings.Core.Services.Ads
{
    public class AdMatchSubscriptionService
    {
        private readonly IAdMatchSubscriptionRepository _adMatchSubscriptionRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly AdService _adService;

        public AdMatchSubscriptionService(
            IAdRepository adRepository,
            IAdMatchSubscriptionRepository adMatchSubscriptionRepository,
            IAdTimeSlotRepository adTimeSlotRepository,
            INotificationRepository notificationRepository
        )
        {
            _adMatchSubscriptionRepository = adMatchSubscriptionRepository;
            _notificationRepository = notificationRepository;
            _adService = new(adRepository, adTimeSlotRepository);
        }

        public void Create(AdMatchSubscription subscription)
            => _adMatchSubscriptionRepository.Create(subscription);

        public bool IsSubscribed(long adId) => _adMatchSubscriptionRepository.ExistsWithAdId(adId);

        public void NotifyAuthorsOfSubscribedMatchingAds(Ad referenceAd)
        {
            var ads = _adService.GetMatchingAds(referenceAd);
            var authorIds = ads.Select(ad => ad.Author.Id).Distinct();

            var notifications = authorIds.Select(
                authorId => Notification.Create(
                        recipientId: authorId,
                        title: "Pronađen kompatibilan oglas",
                        message: "Na oglasnoj tabli se pojavio oglas sa odgovarajućom kategorijom i podudarnim datumima.",
                        type: NotificationType.AdEvent,
                        details: new AdEventNotificationDetails(
                            referenceAd.Id,
                            AdEventType.MatchingAdCreated
                        )
                )
            ).ToList();

            _notificationRepository.CreateMany(notifications);

            _adMatchSubscriptionRepository.DeleteManyByAdId(
                ads.Select(ad => ad.Id).ToList()
            );
        }
    }
}
