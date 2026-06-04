using CommunityHub.Application.Services.Dtos.CommonRooms;
using CommunityHub.Ui.Helpers.Alerts;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident.Buildings.CommonRooms;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.CommonRooms.Details.Requests
{
    public partial class CommonRoomRentalRequestDetailsPage : MainNavigationConsumerPage, IAlertServiceConsumer
    {
        public CommonRoomRentalRequestDetailsPage(CommonRoomRentalRequestDto request)
        {
            InitializeComponent();
            DataContext = new CommonRoomRentalRequestDetailsViewModel(request, this);
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e)
            => Pop();
    }
}
