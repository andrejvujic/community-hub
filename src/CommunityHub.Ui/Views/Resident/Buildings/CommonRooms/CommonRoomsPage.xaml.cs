using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident.Buildings.CommonRooms;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.CommonRooms
{
    public partial class CommonRoomsPage : MainNavigationConsumerPage
    {
        private readonly CommonRoomsViewModel _viewModel;

        public CommonRoomsPage(long userId, long buildingId)
        {
            _viewModel = new CommonRoomsViewModel(userId, buildingId, this);

            InitializeComponent();
            DataContext = _viewModel;

            _viewModel.UpdateCommonRooms();
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e)
            => Pop();
    }
}
