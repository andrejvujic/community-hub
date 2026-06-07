using System.Windows;
using System.Windows.Input;
using CommunityHub.Application.Services.Neighborhoods.CityFacilities;
using CommunityHub.Ui.ViewModels.Citizen.LocalFacilities;

namespace CommunityHub.Ui.Views.Citizen.LocalFacilities
{
    public partial class FacilityVisitsWindow : Window
    {
        public FacilityVisitsWindow(long facilityId, string facilityName, FacilityReservationService facilityReservationService)
        {
            InitializeComponent();
            var viewModel = new FacilityVisitsViewModel(facilityId, facilityName, facilityReservationService);
            DataContext = viewModel;

            if (viewModel != null)
            {
                viewModel.CloseRequested += (_, _) => { Close(); };
            }

            Loaded += (_, _) => { VisitDatesListBox.Focus(); };
        }

        private void Header_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
