using CommunityHub.Application.Services.Dtos.CommonRooms;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident.Buildings.CommonRooms;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.CommonRooms.Details.Requests
{
    public partial class CommonRoomRentalRequestsPage : MainNavigationConsumerPage
    {
        private readonly CommonRoomRentalRequestsViewModel _viewModel;

        public CommonRoomRentalRequestsPage(long userId, CommonRoomDto commonRoom)
        {
            InitializeComponent();
            _viewModel = new CommonRoomRentalRequestsViewModel(userId, commonRoom, this);
            DataContext = _viewModel;

            _viewModel.UpdateRenalRequests();
        }

        private void RequestCommonRoomRentalButton_Click(object sender, RoutedEventArgs e)
            => _viewModel.OpenRequestCommonRoomRentalPage();

        private void RentalRequestItem_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is not FrameworkElement element) return;

            if (element.DataContext is not CommonRoomRentalRequestDto request) return;

            _viewModel.OpenRentalRequestDetailsPage(request);
        }
    }
}
