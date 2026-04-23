using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain.NoticeBoard;
using CommunityHub.Ui.Helpers.Navigation;
using System.Collections.ObjectModel;

namespace CommunityHub.Ui.Views.Resident.Buildings.NoticeBoard
{
    public partial class AdTimeSlotsPage : MainNavigatorConsumerPage
    {
        private readonly long _userId;
        private readonly AdTimeSlotRepository _timeSlotRepository = new();

        public Ad Ad { get; }
        public ObservableCollection<AdTimeSlotGroup> TimeSlotGroups { get; } = new();

        public AdTimeSlotsPage(long userId, Ad ad)
        {
            InitializeComponent();
            DataContext = this;

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
    }
}
