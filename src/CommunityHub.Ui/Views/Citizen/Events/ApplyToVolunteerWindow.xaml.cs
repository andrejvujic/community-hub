using System.Collections.Generic;
using System.Windows;
using CommunityHub.Application.Domain;
using CommunityHub.Application.Database.Repositories;

namespace CommunityHub.Ui.Views.Citizen.Events
{
    public partial class ApplyToVolunteerWindow : Window
    {
        public string EventName { get; set; } = string.Empty;
        public string CitizenName { get; set; } = string.Empty;
        public DateTime EventStart { get; set; }

        private long EventId;
        private CommunityHub.Application.Domain.Citizen? citizen;
        private readonly CitizenRepository _citizenRepository = new CitizenRepository();
        private readonly CommunityEventRepository _eventRepository = new CommunityEventRepository();
        private readonly EventRegistrationRepository _eventRegistrationRepository = new EventRegistrationRepository();
        private readonly EventItemAssignmentRepository _eventItemAssignmentRepository = new EventItemAssignmentRepository();
        private readonly EventItemRepository _eventItemRepository = new EventItemRepository();

        public ApplyToVolunteerWindow(long userId, CommunityEvent ev)
        {
            InitializeComponent();

            EventId = ev.Id;
            EventName = ev.Name;
            EventStart = ev.EventStart;

            citizen = _citizenRepository!.GetById(userId);
            CitizenName = citizen != null ? $"Građanin: {citizen.Name} {citizen.Surname}" : "Građanin";

            this.DataContext = this;

            var itemSelections = new List<EventItemSelection>();
            foreach (var item in ev.Items)
                itemSelections.Add(new EventItemSelection(item));
            ItemsListBox.ItemsSource = itemSelections;

            var assigns = _eventItemAssignmentRepository.GetByEventId(ev.Id);
            var assignedIds = assigns.Select(a => a.Item.Id).ToList();
            ItemsListBox.Tag = assignedIds;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            var registration = new EventRegistration(0, citizen!, EventId, null);
            registration = _eventRegistrationRepository.Create(registration);

            foreach (var selection in ItemsListBox.Items)
            {
                if (selection is EventItemSelection sel && sel.IsChecked)
                {
                    var assignment = new EventItemAssignment(0, registration, sel.Item);
                    _eventItemAssignmentRepository.Create(assignment);
                }
            }

            UpdateEventStatusIfReady();
            this.DialogResult = true;
        }

        private void UpdateEventStatusIfReady()
        {
            var registrations = _eventRegistrationRepository.GetByEventId(EventId);
            var assignments = _eventItemAssignmentRepository.GetByEventId(EventId);
            var items = _eventItemRepository.GetByEventId(EventId);
            var ev = _eventRepository.GetById(EventId);

            bool enoughVolunteers = registrations.Count >= ev!.MinVolunteers;
            bool allItemsAssigned = items.All(i => assignments.Any(a => a.Item.Id == i.Id));
            if (enoughVolunteers && allItemsAssigned && ev.Status == EventStatus.Preparation)
            {
                _eventRepository.UpdateStatus(EventId, EventStatus.Scheduled);
            }
        }
    }

    public class EventItemSelection
    {
        public EventItem Item { get; set; }
        public bool IsChecked { get; set; }
        public EventItemSelection(EventItem item)
        {
            Item = item;
            IsChecked = false;
        }
    }
}
