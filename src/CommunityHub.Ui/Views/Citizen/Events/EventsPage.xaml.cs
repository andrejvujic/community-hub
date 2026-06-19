using CommunityHub.Ui.ViewModels.Citizen.Events;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace CommunityHub.Ui.Views.Citizen.Events
{
    public partial class EventsPage : Page
    {
        private EventsViewModel? _viewModel;

        public EventsPage(long userId)
        {
            InitializeComponent();

            Loaded += (_, _) =>
            {
                _viewModel = new EventsViewModel(userId);
                _viewModel.ApplyToVolunteerRequested += (_, _) => HandleApplyToVolunteer();
                _viewModel.MyEventsRequested += (_, _) => HandleMyEvents();
                DataContext = _viewModel;
            };
        }

        private void HandleApplyToVolunteer()
        {
            var wnd = new ApplyToVolunteerWindow(_viewModel!.GetCitizenId(), _viewModel.SelectedEvent!.Id);
            wnd.Owner = Window.GetWindow(this);
            if (wnd.ShowDialog() == true) _viewModel.RefreshEvents();
        }

        private void HandleMyEvents()
        {
            var wnd = new MyEventsWindow(_viewModel?.GetCitizenId() ?? 0);
            wnd.Owner = Window.GetWindow(this);
            if (wnd.ShowDialog() == true) _viewModel?.RefreshEvents();
        }
    }
}
