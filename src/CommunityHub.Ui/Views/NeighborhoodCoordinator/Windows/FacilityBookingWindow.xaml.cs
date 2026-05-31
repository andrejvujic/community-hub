using System.Windows;
using CommunityHub.Application.Domain.Neighborhoods.CityFacilities;
using CommunityHub.Application.Services.Dtos.Neighborhoods;
using CommunityHub.Application.Services.Dtos.Neighborhoods.CityFacilities;
using CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Tabs;
using CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Windows;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Windows
{
    public partial class FacilityBookingWindow : Window
    {
        public FacilityBookingWindow(
            CityFacilityDto facility,
            NeighborhoodDto neighborhood,
            AvailableFacilitySlot? slotInRange,
            List<AvailableFacilitySlot> slotsOutsideRange,
            CityFacilitiesTabViewModel tabViewModel)
        {
            InitializeComponent();
            DataContext = new FacilityBookingWindowViewModel(
                facility, neighborhood, slotInRange, slotsOutsideRange, tabViewModel,
                OnConfirmed, OnCancelled);
        }

        private void OnConfirmed() 
        { 
            DialogResult = true; 
            Close(); 
        }
        private void OnCancelled() 
        { 
            DialogResult = false; 
            Close(); 
        }
    }
}