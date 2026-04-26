using CommunityHub.Application.Domain.CommonRoom;
using CommunityHub.Ui.Helpers.Navigation;
using System.Collections.ObjectModel;

namespace CommunityHub.Ui.Views.Resident.Buildings.CommonRooms
{
    public partial class CommonRoomsPage : MainNavigatorConsumerPage
    {
        private readonly long _userId, _buildingId;

        public ObservableCollection<CommonRoom> CommonRooms { get; } = new();
        public bool HasCommonRooms => CommonRooms.Count > 0;

        public CommonRoomsPage(long userId, long buildingId)
        {
            _userId = userId;
            _buildingId = buildingId;

            InitializeComponent();
            DataContext = this;
        }
        private void BackButton_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
            => Pop();
    }
}
