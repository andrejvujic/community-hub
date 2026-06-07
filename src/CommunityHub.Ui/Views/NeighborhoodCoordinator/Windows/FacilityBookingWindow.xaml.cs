using System.Windows;
using CommunityHub.Application.Services.Dtos.Neighborhoods;
using CommunityHub.Application.Services.Dtos.Neighborhoods.CityFacilities;
using CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Tabs;
using CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Windows;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Windows
{
    public partial class FacilityBookingWindow : Window
    {
        private readonly FacilityBookingWindowViewModel _viewModel;

        public FacilityBookingWindow(
            CityFacilityDto facility,
            NeighborhoodDto neighborhood,
            AvailableFacilitySlotDto? slotInRange,
            List<AvailableFacilitySlotDto> slotsOutsideRange,
            CityFacilitiesTabViewModel tabViewModel)
        {
            InitializeComponent();

            _viewModel= new FacilityBookingWindowViewModel(
                facility, neighborhood, slotInRange, slotsOutsideRange, tabViewModel);
            
            DataContext = _viewModel;
        }
    }
}