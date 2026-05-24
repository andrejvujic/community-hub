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
        private readonly CommonRoomRentalRequestDetailsViewModel _viewModel;
        public CommonRoomRentalRequestDetailsPage(CommonRoomRentalRequestDto request)
        {
            InitializeComponent();
            _viewModel = new CommonRoomRentalRequestDetailsViewModel(request, this);
            DataContext = _viewModel;
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e)
            => Pop();

        private void CancelRequestButton_Click(object sender, RoutedEventArgs e)
            => _viewModel.ConfirmCancelRequest();

        private void AcceptProposedDateRangeButton_Click(object sender, RoutedEventArgs e)
            => _viewModel.AcceptProposedDateRange();
    }
}
