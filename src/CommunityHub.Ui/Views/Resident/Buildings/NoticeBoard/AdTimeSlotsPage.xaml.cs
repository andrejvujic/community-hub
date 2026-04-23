using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain.NoticeBoard;
using CommunityHub.Ui.Helpers.Navigation;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.NoticeBoard
{
    public partial class AdTimeSlotsPage : MainNavigatorConsumerPage, INotifyPropertyChanged
    {
        private readonly long _userId;
        private readonly AdTimeSlotRepository _timeSlotRepository = new();

        public Ad Ad { get; }
        public ObservableCollection<AdTimeSlotGroup> TimeSlotGroups { get; } = new();
        public ObservableCollection<long> SelectedTimeSlotIds { get; } = new();

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

        private void TimeSlotItem_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is not FrameworkElement element) return;

            if (element.DataContext is not AdTimeSlot timeSlot) return;

            if (SelectedTimeSlotIds.Contains(timeSlot.Id))
            {
                SelectedTimeSlotIds.Remove(timeSlot.Id);
            } else
                SelectedTimeSlotIds.Add(timeSlot.Id);

            OnPropertyChanged(nameof(SelectedTimeSlotIds));
            OnPropertyChanged(nameof(HasSelectedTimeSlots));
        }

        public bool HasSelectedTimeSlots => SelectedTimeSlotIds.Count > 0;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void ReserveButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
