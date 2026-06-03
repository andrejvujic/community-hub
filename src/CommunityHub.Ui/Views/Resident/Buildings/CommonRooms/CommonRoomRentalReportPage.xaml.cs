using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident.Buildings.CommonRooms;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.CommonRooms
{
    public partial class CommonRoomRentalReportPage : MainNavigationConsumerPage
    {
        public CommonRoomRentalReportPage(long userId, long buildingId)
        {
            InitializeComponent();
            DataContext = new CommonRoomRentalReportViewModel(userId, buildingId, this);
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e) => Pop();

        private void ToggleOverlayVisibility(FrameworkElement overlay)
            =>
                overlay.Visibility = overlay.Visibility switch
                {
                    Visibility.Visible => Visibility.Collapsed,
                    Visibility.Collapsed => Visibility.Visible,
                    _ => Visibility.Visible
                };

        private void ToggleDatePickerButton_Click(object sender, EventArgs e)
            => ToggleOverlayVisibility(DatePickerOverlay);
    }
}
