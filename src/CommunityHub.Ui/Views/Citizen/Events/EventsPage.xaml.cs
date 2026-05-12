using System.Windows.Controls;
using CommunityHub.Application.Database.Repositories;
using System.Windows;
using CommunityHub.Application.Domain.Events;

namespace CommunityHub.Ui.Views.Citizen.Events
{
    public partial class EventsPage : Page
    {
        private readonly long _userId;
        private readonly CitizenRepository _citizenRepository = new CitizenRepository();
        private readonly CommunityEventRepository _eventRepository = new CommunityEventRepository();
        private readonly EventRegistrationRepository _eventRegistrationRepository = new EventRegistrationRepository();

        public EventsPage(long userId)
        {
            InitializeComponent();
            _userId = userId;
            var citizen = _citizenRepository.GetById(_userId);
            this.DataContext = citizen;

            RefreshNeighborhoodEvents();
        }

        private void RefreshNeighborhoodEvents()
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
            var selected = EventsGrid.SelectedItem as CommunityEvent;
            if (selected == null)
            {
                MessageBox.Show("Izaberite događaj pre prijave.", "Obaveštenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (selected.Status != EventStatus.Preparation)
            {
                MessageBox.Show("Prijava je dozvoljena samo dok je događaj u pripremi.", "Obaveštenje", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var registrations = _eventRegistrationRepository.GetByEventId(selected.Id);
            var citizen = _citizenRepository.GetById(_userId);
            if (registrations.Any(r => r.Citizen.Id == citizen!.Id))
            {
                MessageBox.Show("Već ste se prijavili za ovaj događaj.", "Obaveštenje", MessageBoxButton.OK, MessageBoxImage.Information);
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
