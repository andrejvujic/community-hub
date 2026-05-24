using CommunityHub.Application;
using CommunityHub.Application.Domain.Ads;
using CommunityHub.Application.Domain.RepositoryInterfaces.Ads;
using CommunityHub.Application.Services.Ads;
using CommunityHub.Application.Services.Dtos.Ads;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.Views.Resident.Buildings.Ads.OtherAds;
using System.Collections.ObjectModel;

namespace CommunityHub.Ui.ViewModels.Resident.Buildings.Ads
{
    public class MyAdTimeSlotsViewModel : BaseViewModel
    {
        private readonly AdTimeSlotService _timeSlotService;

        public ObservableCollection<AdReservationDto> Reservations { get; } = new();
        public bool HasReservations { get => Reservations.Count > 0; }

        private readonly long _userId;
        public AdDto Ad { get; }

        public MyAdTimeSlotsViewModel(
            long userId,
            AdDto ad,
            IMainNavigationConsumer navigationConsumer
        ) : base(navigationConsumer)
        {
            _timeSlotService = new(
                Injector.CreateInstance<IAdTimeSlotRepository>()
            );

            _userId = userId;
            Ad = ad;

            OpenAdDetailsCommand = new(OpenAdDetailsPage);
        }

        public RelayCommand<AdReservationDto> OpenAdDetailsCommand { get; }

        private void OpenAdDetailsPage(AdReservationDto? reservation)
        {
            if (reservation is null) return;

            Push(
                new AdDetailsPage(_userId, reservation.Ad),
                onPop: _ => UpdateReservations()
            );
        }

        public void UpdateReservations()
        {
            var timeSlots = _timeSlotService.GetReservedTimeSlotsForAd(Ad.Id);
            var reservations = GroupTimeSlotsByAd(timeSlots);

            Reservations.Clear();
            foreach (var reservation in reservations)
                Reservations.Add(reservation);

            OnPropertyChanged(nameof(HasReservations));
        }   

        private List<AdTimeSlotGroupDto> GroupTimeSlotsByDay(List<AdTimeSlotDto> timeSlots)
        {
            var groups = timeSlots.GroupBy(timeSlot => timeSlot.StartTime.Date);

            return groups.Select(timeSlots =>
                        new AdTimeSlotGroupDto(
                            timeSlots.First().StartTime.Date,
                            [.. timeSlots]
                        )
                    ).ToList();
        }

        private List<AdReservationDto> GroupTimeSlotsByAd(List<AdTimeSlotDto> timeSlots)
        {
            var groups = timeSlots.GroupBy(timeSlot => {
                var reservation = timeSlot.Reservation!;
                return Ad.Type is AdType.Offer ? reservation.RequestAd!.Id : reservation.OfferAd!.Id;
            });

            return groups.Select(timeSlots => {
                var reservation = timeSlots.First().Reservation!;

                return new AdReservationDto(
                    Ad.Type is AdType.Offer ? reservation.RequestAd! : reservation.OfferAd!,
                    GroupTimeSlotsByDay([.. timeSlots])
                );
            }).ToList();
        }

    }
}
