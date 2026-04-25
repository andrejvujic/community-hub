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

            RefreshNeighborhoodEvents();
        }

        public void RefreshNeighborhoodEvents()
        {
            var citizen = _citizenRepository.GetById(_userId);
            if (citizen?.Membership != null)
            {
                var neighborhoodId = citizen.Membership.Neighborhood.Id;
                var events = _eventRepository.GetByNeighborhoodId(neighborhoodId);
                EventsGrid.ItemsSource = events;
            }
        }

        private void MyEventsButton_Click(object sender, RoutedEventArgs e)
        {
            var wnd = new MyEventsWindow(_userId);
            wnd.Owner = Window.GetWindow(this);
            var result = wnd.ShowDialog();
            if (result == true)
            {
                RefreshNeighborhoodEvents();
            }
        }

        private void ApplyToVolunteerButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var selected = EventsGrid.SelectedItem as CommunityHub.Application.Domain.CommunityEvent;
            if (selected == null)
            {
                MessageBox.Show("Izaberite događaj pre prijave.", "Obaveštenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var wnd = new ApplyToVolunteerWindow(_userId, selected);
            wnd.Owner = Window.GetWindow(this);
            var result = wnd.ShowDialog();
            if (result == true)
            {
                RefreshNeighborhoodEvents();
            }
        }
    }
}
