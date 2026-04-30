using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain.Buildings.Ads;
using CommunityHub.Application.Domain.Buildings.Ads.TimeSlots;
using CommunityHub.Ui.Helpers.Alerts;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.Views.Resident.Buildings.Ads.OtherAds;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.Ads.Browse.MyAds
{
    public partial class MyAdTimeSlotsPage : MainNavigatorConsumerPage, INotifyPropertyChanged, IAlertConsumer
    {
        private readonly long _userId;

        private readonly AdTimeSlotRepository _timeSlotRepository = new();

        public Ad Ad { get; }

        // Contains groups (time slots grouped by day) of time slots for the given
        // notice board that the user can reserve (the user has matching time slots for
        // an ad of the opposite type in the same category).
        public bool HasReservations { get => Reservations.Count > 0; }
        public ObservableCollection<AdReservation> Reservations { get; } = new();

        public MyAdTimeSlotsPage(long userId, Ad ad)
        {
            InitializeComponent();
            DataContext = this;

            _userId = userId;
            Ad = ad;

            UpdateReservations();
        }

        private List<AdTimeSlotGroup> GroupTimeSlotsByDay(List<AdTimeSlot> timeSlots)
        {
            var groups = timeSlots.GroupBy(timeSlot => timeSlot.StartTime.Date);

            return groups.Select(timeSlots =>
                        new AdTimeSlotGroup(
                            timeSlots.First().StartTime.Date,
                            [.. timeSlots]
                        )
                    ).ToList();
        }

        private List<AdReservation> ConstructAdReservations(List<AdTimeSlot> timeSlots)
        {
            var groups = timeSlots.GroupBy(timeSlot => {
                var reservation = timeSlot.Reservation!;
                return Ad.IsOffer ? reservation.RequestAd.Id : reservation.OfferAd.Id;
            });

            return groups.Select(timeSlots =>
            {
                var reservation = timeSlots.First().Reservation!;
                return new AdReservation(
                    Ad.IsOffer ? reservation.RequestAd : reservation.OfferAd,
                    GroupTimeSlotsByDay([.. timeSlots])
                );
            }).ToList();
        }

        private void UpdateReservations()
        {
            var timeSlots = _timeSlotRepository.GetReservedTimeSlots(Ad.Id);
            Debug.WriteLine(timeSlots.Count);

            Reservations.Clear();
            foreach (var reservations in ConstructAdReservations(timeSlots))
                Reservations.Add(reservations);
        }

        private void ReservationItem_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is not FrameworkElement element) return;

            if (element.DataContext is not AdReservation reservation) return;

            Push(
                new AdDetailsPage(_userId, reservation.Ad),
                onPop: _ => UpdateReservations()
            );
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
