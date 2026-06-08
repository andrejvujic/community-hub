using CommunityHub.Application;
using CommunityHub.Application.Domain.Notifications;
using CommunityHub.Application.Domain.Notifications.Details;
using CommunityHub.Application.Domain.Notifications.RepositoryInterfaces;
using CommunityHub.Application.Domain.RepositoryInterfaces.Ads;
using CommunityHub.Application.Services.Ads;
using CommunityHub.Application.Services.Dtos.Ads;
using CommunityHub.Application.Services.Dtos.Notifications;
using CommunityHub.Application.Services.Notifications;
using CommunityHub.Ui.Helpers.Alerts;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Helpers.Navigation;
using System.Collections.ObjectModel;

namespace CommunityHub.Ui.ViewModels.Resident.Buildings.Ads
{
    public class AdTimeSlotsViewModel : BaseViewModel
    {
        private readonly NotificationService _notificationService;

        private readonly AdTimeSlotService _timeSlotService;
        private readonly AdTimeSlotReservationService _timeSlotReservationService;

        public long UserId { get; }
        public AdDto Ad { get; }

        public bool HasReservableTimeSlots { get => ReservableTimeSlotGroups.Count > 0; }
        public ObservableCollection<AdTimeSlotGroupDto> ReservableTimeSlotGroups { get; } = new();

        public bool HasSelectedTimeSlots => SelectedTimeSlots.Count > 0;
        public ObservableCollection<AdTimeSlotDto> SelectedTimeSlots { get; } = new();

        public AdTimeSlotsViewModel(long userId, AdDto ad, MainNavigationConsumerPage page) : base(page) {
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

            ReserveCommand = new(Reserve);
            OnTimeSlotClickCommand = new(OnTimeSlotClick);
        }

        public RelayCommand ReserveCommand { get; }
        public RelayCommand<AdTimeSlotDto> OnTimeSlotClickCommand { get; }

        public void Reserve()
        {
            if (!Ad.IsActive)
            {
                ShowErrorAlert();
                DeselectAllTimeSlots();
                return;
            }

            ReserveSelectedTimeSlots();

            ShowSuccessAlert();

            UpdateReservableTimeSlots();

            DeselectAllTimeSlots();
        }

        private void ShowErrorAlert()
            => ShowAlert(
                    Alert.Create(
                        "Rezervacija nije moguća",
                        "Rezervacija termina nije moguća za ovaj oglas zato što je arhiviran.",
                        type: AlertType.Error
                    )
                    .AddOption("U redu")
                );

        private void ShowSuccessAlert()
            => ShowAlert(
                Alert.Create(
                    "Rezervacija uspješna",
                    "Uspješno ste rezervisali termine. Autor oglasa je obaviješten o vašoj rezervaciji."
                )
                .AddOption("U redu")
            );

        private void ReserveSelectedTimeSlots()
        {
            var reservations = _timeSlotReservationService.Reserve(
                UserId,
                [.. SelectedTimeSlots]
            );

            _notificationService.Create(
                new NotificationDto(
                    recipientId: Ad.AuthorId,
                    title: "Nova rezervacija",
                    message: "Napravljena je nova rezervacija za vaš oglas.",
                    type: NotificationType.AdEvent,
                    details: new AdEventNotificationDetailsDto(
                        Ad.Id,
                        AdEventType.AdTimeSlotsReserved
                    ),
                    sentAt: DateTime.Now
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

        private static List<AdTimeSlotGroupDto> GroupTimeSlotsByDay(List<AdTimeSlotDto> timeSlots)
        {
            var groups = timeSlots.GroupBy(timeSlot => timeSlot.StartTime.Date);

            return groups.Select(timeSlots =>
                        new AdTimeSlotGroupDto(
                            timeSlots.First().StartTime.Date,
                            [.. timeSlots]
                        )
                    ).ToList();
        }

        private void OnTimeSlotClick(AdTimeSlotDto? timeSlot)
        {
            if (timeSlot is null || timeSlot.IsReserved) return;

            if (IsTimeSlotSelected(timeSlot))
            {
                DeselectTimeSlot(timeSlot);
                return;
            }
 
            SelectTimeSlot(timeSlot);
        }

        public bool IsTimeSlotSelected(AdTimeSlotDto timeSlot)
        {
            return SelectedTimeSlots.Contains(timeSlot);
        }

        public void SelectTimeSlot(AdTimeSlotDto timeSlot)
        {
            SelectedTimeSlots.Add(timeSlot);

            OnPropertyChanged(nameof(SelectedTimeSlots));
            OnPropertyChanged(nameof(HasSelectedTimeSlots));
        }

        public void DeselectTimeSlot(AdTimeSlotDto timeSlot)
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

    }
}
