using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain.RepositoryInterfaces;
using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.RepositoryInterfaces;
using CommunityHub.Application.Modules.Buildings.Database.Repositories.Ads;
using CommunityHub.Application.Modules.Buildings.Database.Repositories.Ads.TimeSlots;
using CommunityHub.Application.Modules.Shared.Core.Domain.Notifications.RepositoryInterfaces;
using CommunityHub.Application.Modules.Shared.Core.Domain.Users.RepositoryInterfaces;
using CommunityHub.Application.Modules.Shared.Database.Repositories.Users;
using CommunityHub.Application.Shared.Database.Repositories.Notifications;

namespace CommunityHub.Application
{
    public class Injector
    {
        private static Dictionary<Type, object> _implementations = new()
        {
            { typeof(IUserRepository), new UserRepository() },
            { typeof(IAdRepository), new AdRepository() },
            { typeof(IAdTimeSlotRepository), new AdTimeSlotRepository() },
            { typeof(IAdTimeSlotReservationRepository), new AdTimeSlotReservationRepository() },
            { typeof(IAdMatchSubscriptionRepository), new AdMatchSubscriptionRepository() },
            { typeof(IBuildingRepository), new BuildingRepository() },
            { typeof(IApartmentRepository), new ApartmentRepository() },
            { typeof(IApartmentAccessRequestRepository), new ApartmentAccessRequestRepository() },
            { typeof(IApartmentMembershipRepository), new ApartmentMembershipRepository() },
            { typeof(ICommonRoomRepository), new CommonRoomRepository() },
            { typeof(ICommonRoomRentalRequestRepository), new CommonRoomRentalRequestRepository() },
            { typeof(IIssueRepository), new IssueRepository() },
            { typeof(INotificationRepository), new NotificationRepository() },
            { typeof(INotificationDetailsRepository), new NotificationDetailsRepository() },
            { typeof(IAddressRepository), new AddressRepository() },
            { typeof(ICityRepository), new CityRepository() },
            { typeof(ICountryRepository), new CountryRepository() },
            { typeof(IStreetRepository), new StreetRepository() },
            { typeof(IStreetSectionRepository), new StreetSectionRepository() },
            { typeof(IImageRepository), new ImageRepository() },
        };

        public static T CreateInstance<T>()
        {
            var type = typeof(T);

            if (_implementations.ContainsKey(type))
                return (T)_implementations[type];

            throw new ArgumentException($"No implementation found for type {type}");
        }
    }

}
