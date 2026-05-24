using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Database.Repositories.Ads;
using CommunityHub.Application.Database.Repositories.Ads.TimeSlots;
using CommunityHub.Application.Database.Repositories.Apartments;
using CommunityHub.Application.Database.Repositories.Buildings;
using CommunityHub.Application.Database.Repositories.CommonRooms;
using CommunityHub.Application.Database.Repositories.Issues;
using CommunityHub.Application.Database.Repositories.Events;
using CommunityHub.Application.Database.Repositories.Neighborhoods;
using CommunityHub.Application.Database.Repositories.Shared;
using CommunityHub.Application.Database.Repositories.Shared.Addresses;
using CommunityHub.Application.Database.Repositories.Shared.Notifications;
using CommunityHub.Application.Domain.Notifications.RepositoryInterfaces;
using CommunityHub.Application.Database.Repositories.Neighborhoods.NeighborhoodMeetings;
using CommunityHub.Application.Database.Repositories.JobListings;
using CommunityHub.Application.Database.Repositories.JobListingBookings;
using CommunityHub.Application.Domain.RepositoryInterfaces.Addresses;
using CommunityHub.Application.Domain.RepositoryInterfaces.Ads;
using CommunityHub.Application.Domain.RepositoryInterfaces.Apartments;
using CommunityHub.Application.Domain.RepositoryInterfaces.Buildings;
using CommunityHub.Application.Domain.RepositoryInterfaces.CommonRooms;
using CommunityHub.Application.Domain.RepositoryInterfaces.Images;
using CommunityHub.Application.Domain.RepositoryInterfaces.Issues;
using CommunityHub.Application.Domain.RepositoryInterfaces.Users;
using CommunityHub.Application.Domain.RepositoryInterfaces.Neighborhoods;
using CommunityHub.Application.Domain.RepositoryInterfaces.JobListings;
using CommunityHub.Application.Domain.RepositoryInterfaces.Neighborhoods.NeighborhoodMeetings;
using CommunityHub.Application.Domain.RepositoryInterfaces.JobListingBookings;
using CommunityHub.Application.Domain.RepositoryInterfaces.Events;

namespace CommunityHub.Application
{
    public class Injector
    {
        private static Dictionary<Type, object> _implementations = new()
        {
            { typeof(IUserRepository), new UserRepository() },
            { typeof(ICitizenRepository), new CitizenRepository() },
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
            { typeof(ICommunityEventRepository), new CommunityEventRepository() },
            { typeof(IEventItemRepository), new EventItemRepository() },
            { typeof(IEventRegistrationRepository), new EventRegistrationRepository() },
            { typeof(IEventItemAssignmentRepository), new EventItemAssignmentRepository() },
            { typeof(INeighborhoodAccessRequestRepository), new NeighborhoodAccessRequestRepository() },
            { typeof(INeighborhoodRepository), new NeighborhoodRepository() },
            { typeof(INeighborhoodMembershipRepository), new NeighborhoodMembershipRepository() },
            { typeof(INeighborhoodMeetingRepository), new NeighborhoodMeetingRepository() },
            { typeof(IMeetingDateVoteRepository), new MeetingDateVoteRepository() },
            { typeof(IJobListingRepository), new JobListingRepository()  },
            { typeof(IBookedJobRepository), new BookedJobRepository() },
            { typeof(IProviderReviewRepository), new ProviderReviewRepository() }

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
