using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain.NoticeBoard;
using CommunityHub.Ui.Helpers.Alerts;
using CommunityHub.Ui.Helpers.Navigation;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.NoticeBoard
{
    public partial class AdTimeSlotsPage : MainNavigatorConsumerPage, INotifyPropertyChanged, IAlertConsumer
    {
        private readonly long _userId;

        private readonly CultureInfo _culture = new CultureInfo("sr-Latn-RS");

        private readonly AdTimeSlotRepository _timeSlotRepository = new();
        private readonly AdTimeSlotReservationRepository _timeSlotReservationRepository = new();

        public Ad Ad { get; }

        // Contains groups (time slots grouped by day) of time slots for the given
        // notice board that the user can reserve (the user has matching time slots for
        // an ad of the opposite type in the same category).
        public bool HasReservableTimeSlots { get => TimeSlotGroups.Count > 0; }
        public ObservableCollection<AdTimeSlotGroup> TimeSlotGroups { get; } = new();

        // Collection of time slots selected by the user to be reserved.
        public ObservableCollection<AdTimeSlot> SelectedTimeSlots { get; } = new();


        public AdTimeSlotsPage(long userId, Ad ad)
        {
            InitializeComponent();
            DataContext = this;

            _userId = userId;
            Ad = ad;

            UpdateReservableTimeSlots();
        }

        private List<AdTimeSlotGroup> GroupTimeSlotsByDay(List<AdTimeSlot> timeSlots)
        {
            var groups = timeSlots.GroupBy(timeSlot => timeSlot.StartTime.Date);

            return groups.Select(timeSlots =>
                        new AdTimeSlotGroup(
                            timeSlots.First().StartTime.Date,
                            [..timeSlots]
                        )
                    ).ToList();
        }


        private void UpdateReservableTimeSlots()
        {
            var timeSlots = _timeSlotRepository.GetReservableTimeSlotsForUser(
                Ad.Id,
                _userId
            );

            TimeSlotGroups.Clear();
            foreach (var group in GroupTimeSlotsByDay(timeSlots))
                TimeSlotGroups.Add(group);
        }

        private List<AdTimeSlotReservation>? CreateReservations()
        {
            var reservations = new List<AdTimeSlotReservation>();

            foreach (var selectedTimeSlot in SelectedTimeSlots)
            {
                var matchingTimeSlot = _timeSlotRepository.GetMatchingTimeSlotInUserAds(
                    selectedTimeSlot.Id,
                    _userId
                );

                // If the user doesn't have an available (not reserved) time slot
                // they can't reserve the selected time slot.
                if (matchingTimeSlot == null)
                {
                    if (this is IAlertConsumer consumer)
                        consumer.ShowAlert(
                            Alert.Create(
                                "Greška",
                                $"Nije bilo moguće rezervisati termin {selectedTimeSlot.StartTime:HH.mm} — {selectedTimeSlot.EndTime:HH.mm} na dan {selectedTimeSlot.StartTime.ToString("d. MMMM", _culture)} zato što već imate dogovor sklopljen sa drugim stanarom u ovom terminu.",
                                AlertType.Error
                            ).AddOption("U redu")
                        );
                    return null;
                }

                reservations.Add(
                    new AdTimeSlotReservation(
                        selectedTimeSlot.Id,
                        matchingTimeSlot.Id
                    )
                );
            }

            return reservations;
        }

        private void ReserveSelectedTimeSlots()
        {
            var reservations = CreateReservations();

            if (reservations == null) return;

            foreach (var reservation in reservations)
                _timeSlotReservationRepository.Create(reservation);

            if (this is IAlertConsumer consumer)
                consumer.ShowAlert(
                    Alert.Create(
                        "Rezervacija uspješna",
                        "Uspješno ste rezervisali termine. Autor oglasa je obaviješten o vašoj rezervaciji."
                    ).AddOption("U redu")
                );
        }

        private void ReserveButton_Click(object sender, RoutedEventArgs e)
        {
            ReserveSelectedTimeSlots();

            UpdateReservableTimeSlots();

            SelectedTimeSlots.Clear();
            OnPropertyChanged(nameof(SelectedTimeSlots));
            OnPropertyChanged(nameof(HasSelectedTimeSlots));
        }

        private void TimeSlotItem_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is not FrameworkElement element) return;

            if (element.DataContext is not AdTimeSlot timeSlot || timeSlot.IsReserved) return;

            if (SelectedTimeSlots.Contains(timeSlot))
                SelectedTimeSlots.Remove(timeSlot);
            else
                SelectedTimeSlots.Add(timeSlot);

            OnPropertyChanged(nameof(SelectedTimeSlots));
            OnPropertyChanged(nameof(HasSelectedTimeSlots));
        }

        public bool HasSelectedTimeSlots => SelectedTimeSlots.Count > 0;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
