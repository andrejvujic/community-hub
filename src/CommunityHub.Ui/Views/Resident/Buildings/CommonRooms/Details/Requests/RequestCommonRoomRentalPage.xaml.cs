using CommunityHub.Application.Services.CommonRooms;
using CommunityHub.Application.Services.Dtos.CommonRooms;
using CommunityHub.Ui.Helpers.Alerts;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident.Buildings.CommonRooms;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.CommonRooms.Details.Requests
{
    public partial class RequestCommonRoomRentalPage : MainNavigationConsumerPage, IAlertServiceConsumer
    {
        public RequestCommonRoomRentalPage(long userId, CommonRoomDto commonRoom)
        {
            InitializeComponent();
            DataContext = new RequestCommonRoomRentalViewModel(userId, commonRoom, this);
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

        private void ToggleSelectDateRangeMenuButton_Click(object sender, EventArgs e)
            => ToggleOverlayVisibility(SelectDateRangeOverlay);

        private void ToggleStartDatePickerButton_Click(object sender, EventArgs e)
            => ToggleOverlayVisibility(StartDatePicker);

        private void ToggleEndDatePickerButton_Click(object sender, EventArgs e)
            => ToggleOverlayVisibility(EndDatePicker);

    }
}
