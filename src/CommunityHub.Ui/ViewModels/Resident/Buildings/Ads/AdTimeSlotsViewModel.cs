using CommunityHub.Application;
using CommunityHub.Application.Domain.Ads;
using CommunityHub.Application.Domain.Ads.TimeSlots;
using CommunityHub.Application.Domain.Notifications;
using CommunityHub.Application.Domain.Notifications.Details;
using CommunityHub.Application.Domain.Notifications.RepositoryInterfaces;
using CommunityHub.Application.Domain.RepositoryInterfaces.Ads;
using CommunityHub.Application.Services.Ads;
using CommunityHub.Application.Services.Notifications;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CommunityHub.Ui.ViewModels.Resident.Buildings.Ads
{
    public class AdTimeSlotsViewModel : INotifyPropertyChanged
    {
        private readonly NotificationService _notificationService;

        private readonly AdTimeSlotService _timeSlotService;
        private readonly AdTimeSlotReservationService _timeSlotReservationService;

        public long UserId { get; }
        public Ad Ad { get; }

        public bool HasReservableTimeSlots { get => ReservableTimeSlotGroups.Count > 0; }
        public ObservableCollection<AdTimeSlotGroup> ReservableTimeSlotGroups { get; } = new();

        public bool HasSelectedTimeSlots => SelectedTimeSlots.Count > 0;
        public ObservableCollection<AdTimeSlot> SelectedTimeSlots { get; } = new();

        public AdTimeSlotsViewModel(long userId, Ad ad) {
            _notificationService = new(
                Injector.CreateInstance<INotificationRepository>()
            );

            _timeSlotService = new(
                Injector.CreateInstance<IAdTimeSlotRepository>()
            );

            _timeSlotReservationService = new(
                Injector.CreateInstance<IAdTimeSlotRepository>(),
                Injector.CreateInstance<IAdTimeSlotReservationRepository>()
            );

            UserId = userId;
            Ad = ad;
        }

        public void ReserveSelectedTimeSlots()
        {
            var reservations = _timeSlotReservationService.Reserve(
                UserId,
                [.. SelectedTimeSlots]
            );

            _notificationService.Create(
                Notification.Create(
                    Ad.Author.Id,
                    "Nova rezervacija",
                    "Napravljena je nova rezervacija za vaš oglas.",
                    NotificationType.AdEvent,
                    details: new AdEventNotificationDetails(
                        Ad.Id,
                        AdEventType.AdTimeSlotsReserved
                    )
                )
            );
        }

        public void UpdateReservableTimeSlots()
        {
            var timeSlots = _timeSlotService.GetReservableTimeSlotsForUser(
                Ad.Id,
                UserId
            );

            ReservableTimeSlotGroups.Clear();
            foreach (var group in GroupTimeSlotsByDay(timeSlots))
                ReservableTimeSlotGroups.Add(group);
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

        public bool IsTimeSlotSelected(AdTimeSlot timeSlot)
        {
            return SelectedTimeSlots.Contains(timeSlot);
        }

        public void SelectTimeSlot(AdTimeSlot timeSlot)
        {
            SelectedTimeSlots.Add(timeSlot);

            OnPropertyChanged(nameof(SelectedTimeSlots));
            OnPropertyChanged(nameof(HasSelectedTimeSlots));
        }

        public void DeselectTimeSlot(AdTimeSlot timeSlot)
        {
            if (!IsTimeSlotSelected(timeSlot))
                throw new ArgumentException("Time slot is not selected.");

            SelectedTimeSlots.Remove(timeSlot);

            OnPropertyChanged(nameof(SelectedTimeSlots));
            OnPropertyChanged(nameof(HasSelectedTimeSlots));
        }

        public void DeselectAllTimeSlots()
        {
            SelectedTimeSlots.Clear();
            OnPropertyChanged(nameof(SelectedTimeSlots));
            OnPropertyChanged(nameof(HasSelectedTimeSlots));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
