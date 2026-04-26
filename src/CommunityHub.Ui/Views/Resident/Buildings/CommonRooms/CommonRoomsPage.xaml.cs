using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain.CommonRoom;
using CommunityHub.Ui.Helpers.Navigation;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.CommonRooms
{
    public partial class CommonRoomsPage : MainNavigatorConsumerPage
    {
        private readonly long _userId, _buildingId;
        private readonly CommonRoomRepository _commonRoomRepository = new();

        public ObservableCollection<CommonRoom> CommonRooms { get; } = new();
        public bool HasCommonRooms => CommonRooms.Count > 0;

        public CommonRoomsPage(long userId, long buildingId)
        {
            _userId = userId;
            _buildingId = buildingId;

            InitializeComponent();
            DataContext = this;

            GetCommonRooms();
        }

        private void GetCommonRooms()
        {
            CommonRooms.Clear();
            var commonRooms = _commonRoomRepository.GetByBuildingId(_buildingId);
            foreach (var commonRoom in commonRooms)
                CommonRooms.Add(commonRoom);
        }

        private void CommonRoomItem_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is not FrameworkElement element) return;

            if (element.DataContext is not CommonRoom commonRoom) return;

            Push(new CommonRoomDetailsPage(_userId, commonRoom));
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e)
            => Pop();
    }
}
