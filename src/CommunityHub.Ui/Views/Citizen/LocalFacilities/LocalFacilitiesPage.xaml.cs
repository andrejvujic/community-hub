using System.Windows;
using System.Windows.Controls;
using CommunityHub.Application.Services.Neighborhoods.CityFacilities;
using CommunityHub.Application.Services.Users;
using CommunityHub.Ui.ViewModels.Citizen.LocalFacilities;

namespace CommunityHub.Ui.Views.Citizen.LocalFacilities
{
    public partial class LocalFacilitiesPage : Page
    {
        private LocalFacilitiesViewModel? _viewModel;
        private readonly FacilityReservationService _reservationService = new();
        private readonly CitizenService _citizenService = new();
        private readonly long _userId;

        public LocalFacilitiesPage(long userId)
        {
            InitializeComponent();
            _userId = userId;

            Loaded += (_, _) =>
            {
                _viewModel = new LocalFacilitiesViewModel(userId);
                _viewModel.ViewHistoryRequested += (_, _) => HandleViewHistory();
                _viewModel.ViewStatisticsRequested += (_, _) => HandleViewStatistics();
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

        private void HandleViewStatistics()
        {
            var citizen = _citizenService.GetCitizenById(_userId);
            if (citizen?.Membership?.Neighborhood.Id == null) return;

            var window = new ReservationStatisticsWindow();
            window.Owner = Window.GetWindow(this);
            window.ShowDialog();
        }
    }
}