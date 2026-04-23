using System.Windows;

namespace CommunityHub.Ui.Views.Citizen.Events
{
    public partial class MyEventsWindow : Window
    {
        private readonly long _userId;

        public MyEventsWindow(long userId)
        {
            InitializeComponent();
            _userId = userId;

            var eventRepo = new CommunityHub.Application.Database.Repositories.CommunityEventRepository();
            var events = eventRepo.GetByOrganizerId(_userId);
            OrganizerEventsList.ItemsSource = events;
            var volunteerEvents = eventRepo.GetByVolunteerCitizenId(_userId);
            VolunteerEventsList.ItemsSource = volunteerEvents;
        }

        private void OrganizeButton_Click(object sender, RoutedEventArgs e)
        {
            var wnd = new OrganizeEventWindow(_userId);
            wnd.Owner = this;
            wnd.ShowDialog();
        }
    }
}
