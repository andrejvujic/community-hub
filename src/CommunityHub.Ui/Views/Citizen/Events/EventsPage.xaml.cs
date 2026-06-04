using System.Windows;
using System.Windows.Controls;
using CommunityHub.Ui.ViewModels.Citizen.Events;

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
                _viewModel.PrintReportRequested += (_, _) => HandlePrintReport();
                _viewModel.MyEventsRequested += (_, _) => HandleMyEvents();
                DataContext = _viewModel;
            };
        }

        private void HandleApplyToVolunteer()
        {
            var validationError = _viewModel?.ValidateApplyToVolunteer();
            if (!string.IsNullOrEmpty(validationError))
            {
                MessageBox.Show(validationError, "Obaveštenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var wnd = new ApplyToVolunteerWindow(_viewModel!.GetCitizenId(), _viewModel.SelectedEvent!.Id);
            wnd.Owner = Window.GetWindow(this);
            if (wnd.ShowDialog() == true)
            {
                _viewModel.RefreshEvents();
            }
        }

        private void HandlePrintReport()
        {
            MessageBox.Show("Funkcionalnost štampanja izveštaja će biti implementirana.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void HandleMyEvents()
        {
            var wnd = new MyEventsWindow(_viewModel?.GetCitizenId() ?? 0);
            wnd.Owner = Window.GetWindow(this);
            if (wnd.ShowDialog() == true)
            {
                _viewModel?.RefreshEvents();
            }
        }
    }
}
