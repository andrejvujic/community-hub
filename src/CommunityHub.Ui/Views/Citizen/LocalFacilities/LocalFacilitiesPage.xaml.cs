using System.Windows;
using System.Windows.Controls;
using CommunityHub.Application.Services.Neighborhoods.CityFacilities;
using CommunityHub.Ui.ViewModels.Citizen.LocalFacilities;

namespace CommunityHub.Ui.Views.Citizen.LocalFacilities
{
    public partial class LocalFacilitiesPage : Page
    {
        private LocalFacilitiesViewModel? _viewModel;
        private readonly FacilityReservationService _reservationService = new();

        public LocalFacilitiesPage(long userId)
        {
            InitializeComponent();

            Loaded += (_, _) =>
            {
                _viewModel = new LocalFacilitiesViewModel(userId);
                _viewModel.ViewHistoryRequested += (_, _) => HandleViewHistory();
                DataContext = _viewModel;
            };
        }

        private void HandleViewHistory()
        {
            if (_viewModel?.SelectedFacility == null) return;

            var window = new FacilityVisitsWindow(_viewModel.SelectedFacility.Id, _viewModel.SelectedFacility.Name, _reservationService);
            window.Owner = Window.GetWindow(this);
            window.ShowDialog();
        }
    }
}