using System.Windows;
using CommunityHub.Application.Database.Repositories;

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
    }
}
