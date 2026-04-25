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

        public ApplyToVolunteerWindow(long userId, CommunityEvent ev)
        {
            InitializeComponent();

            EventName = ev.Name;
            EventStart = ev.EventStart;

            CitizenRepository _citizenRepository = new CitizenRepository();
            var citizen = _citizenRepository.GetById(userId);
            CitizenName = citizen != null ? $"Građanin: {citizen.Name} {citizen.Surname}" : "Građanin";

            this.DataContext = this;

            ItemsListBox.ItemsSource = ev.Items;
            EventItemAssignmentRepository _eventItemAssignmentRepository = new EventItemAssignmentRepository();
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
            this.DialogResult = true;
        }

    }
}
