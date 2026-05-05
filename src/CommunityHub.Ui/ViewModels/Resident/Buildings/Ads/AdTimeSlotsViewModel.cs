using CommunityHub.Application;
using CommunityHub.Application.Modules.Buildings.API.Public.Ads.TimeSlots;
using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads;
using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.RepositoryInterfaces;
using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.TimeSlots;
using CommunityHub.Application.Modules.Shared.API.Public.Notifications;
using CommunityHub.Application.Modules.Shared.Core.Domain.Notifications;
using CommunityHub.Application.Modules.Shared.Core.Domain.Notifications.Details;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CommunityHub.Ui.ViewModels.Resident.Buildings.Ads
{
    public class AdTimeSlotsViewModel : INotifyPropertyChanged
    {
        private readonly INotificationService _notificationService
            = Injector.CreateInstance<INotificationService>();

        private readonly IAdTimeSlotService _adTimeSlotService
            = Injector.CreateInstance<IAdTimeSlotService>();

        private readonly IAdTimeSlotReservationService _timeSlotReservationService
            = Injector.CreateInstance<IAdTimeSlotReservationService>();

        public long UserId { get; }
        public Ad Ad { get; }

        public bool HasReservableTimeSlots { get => ReservableTimeSlotGroups.Count > 0; }
        public ObservableCollection<AdTimeSlotGroup> ReservableTimeSlotGroups { get; } = new();

        public bool HasSelectedTimeSlots => SelectedTimeSlots.Count > 0;
        public ObservableCollection<AdTimeSlot> SelectedTimeSlots { get; } = new();

        public AdTimeSlotsViewModel(long userId, Ad ad) {
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
            var timeSlots = _adTimeSlotService.GetReservableTimeSlotsForUser(
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
