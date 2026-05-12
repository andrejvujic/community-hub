using System.Windows;
using System.Windows.Input;
using CommunityHub.Application.Database.Repositories.Events;
using CommunityHub.Application.Domain.Events;

namespace CommunityHub.Ui.Views.Citizen.Events
{
    public partial class MyEventsWindow : Window
    {
        private readonly long _userId;
        private readonly CommunityEventRepository _communityEventRepository = new CommunityEventRepository();

        public MyEventsWindow(long userId)
        {
            InitializeComponent();
            _userId = userId;

            var events = _communityEventRepository.GetByOrganizerId(_userId);
            OrganizerEventsList.ItemsSource = events;
            var volunteerEvents = _communityEventRepository.GetByVolunteerCitizenId(_userId);
            VolunteerEventsList.ItemsSource = volunteerEvents;

            OrganizerEventsList.PreviewKeyDown += OrganizerEventsList_PreviewKeyDown;
        }

        private void OrganizeButton_Click(object sender, RoutedEventArgs e)
        {
            var wnd = new OrganizeEventWindow(_userId);
            wnd.Owner = this;
            var dialogResult = wnd.ShowDialog();
            if (dialogResult == true)
            {
                var events = _communityEventRepository.GetByOrganizerId(_userId);
                OrganizerEventsList.ItemsSource = events;
                this.DialogResult = true;
            }
        }

        private void OrganizerEventsList_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var selectedEvent = OrganizerEventsList.SelectedItem as CommunityEvent;
                if (selectedEvent == null)
                    return;

                if (selectedEvent.Status == EventStatus.Finished && selectedEvent.Organizer.Id == _userId)
                {
                    var wnd = new MarkVolunteersWindow(selectedEvent);
                    wnd.Owner = this;
                    wnd.ShowDialog();
                    e.Handled = true;

                    var events = _communityEventRepository.GetByOrganizerId(_userId);
                    OrganizerEventsList.ItemsSource = events;
                }
            }
        }
    }
}
