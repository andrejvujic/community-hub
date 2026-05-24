using CommunityHub.Application.Services.Dtos.CommonRooms;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident.Buildings.CommonRooms;

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

    }
}
