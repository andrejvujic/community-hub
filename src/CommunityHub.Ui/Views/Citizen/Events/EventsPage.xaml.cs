using System.Windows.Controls;
using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;
using System.Windows;

namespace CommunityHub.Ui.Views.Citizen.Events
{
    public partial class EventsPage : Page
    {
        private readonly long _userId;
        private readonly CitizenRepository _citizenRepository = new CitizenRepository();
        private readonly CommunityEventRepository _eventRepository = new CommunityEventRepository();

        public EventsPage(long userId)
        {
            InitializeComponent();
            _userId = userId;
            var citizen = _citizenRepository.GetById(_userId);
            this.DataContext = citizen;

            if (citizen?.Membership != null)
            {
                var neighborhoodId = citizen.Membership.Neighborhood.Id;
                var events = _eventRepository.GetByNeighborhoodId(neighborhoodId);
                EventsGrid.ItemsSource = events;
            }
        }

        private void MyEventsButton_Click(object sender, RoutedEventArgs e)
        {
            OpenMyEventsWindow();
        }

        private void OpenMyEventsWindow()
        {
            var wnd = new MyEventsWindow();
            wnd.Owner = Window.GetWindow(this);
            wnd.ShowDialog();
        }
    }
}
