using CommunityHub.Application.Domain.CommonRooms;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident.Buildings.CommonRooms;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.CommonRooms
{
    public partial class CommonRoomsPage : MainNavigatorConsumerPage
    {
        private readonly CommonRoomsViewModel _viewModel;

        public CommonRoomsPage(long userId, long buildingId)
        {
            _viewModel = new CommonRoomsViewModel(userId, buildingId);

            InitializeComponent();
            DataContext = _viewModel;

            _viewModel.UpdateCommonRooms();
        }

        private void CommonRoomItem_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is not FrameworkElement element) return;

            if (element.DataContext is not CommonRoom commonRoom) return;

            Push(new CommonRoomDetailsPage(_viewModel.UserId, commonRoom));
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e)
            => Pop();
    }
}
