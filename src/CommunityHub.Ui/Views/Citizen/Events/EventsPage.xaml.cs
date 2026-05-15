using System.Windows.Controls;
using System.Windows;
using CommunityHub.Application.Services.Dtos.Events;
using CommunityHub.Ui.ViewModels.Citizen.Events;

namespace CommunityHub.Ui.Views.Citizen.Events
{
    public partial class EventsPage : Page
    {
        private readonly EventsViewModel _viewModel;

        public EventsPage(long userId)
        {
            InitializeComponent();
            _viewModel = new EventsViewModel(userId);
            DataContext = _viewModel;
        }

        private void MyEventsButton_Click(object sender, RoutedEventArgs e)
        {
            var wnd = new MyEventsWindow(_viewModel.Citizen!.Id);
            wnd.Owner = Window.GetWindow(this);
            var result = wnd.ShowDialog();
            if (result == true)
            {
                _viewModel.RefreshEvents();
            }
        }

        private void ApplyToVolunteerButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedEvent = EventsGrid.SelectedItem as CommunityEventDto;
            if (selectedEvent == null)
            {
                MessageBox.Show("Izaberite događaj pre prijave.", "Obaveštenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var validationError = _viewModel.CanApplyToVolunteer(selectedEvent.Id);
            if (validationError != null)
            {
                MessageBox.Show(validationError, "Obaveštenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var wnd = new ApplyToVolunteerWindow(_viewModel.Citizen!.Id, selectedEvent.Id);
            wnd.Owner = Window.GetWindow(this);
            var result = wnd.ShowDialog();
            if (result == true)
            {
                _viewModel.RefreshEvents();
            }
        }
    }
}

