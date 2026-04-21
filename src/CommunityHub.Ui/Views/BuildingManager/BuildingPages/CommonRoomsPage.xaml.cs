using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.CommonRoom;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace CommunityHub.Ui.Views.BuildingManager.BuildingPages
{
    public partial class CommonRoomsPage : Page
    {
        private readonly CommonRoomRepository _commonRoomRepository = new();
        private readonly Building _building;
        private readonly User _user;
        public ObservableCollection<CommonRoom> CommonRooms { get; set; } = [];
        public CommonRoomsPage(User user, Building building)
        {
            InitializeComponent();
            DataContext = this;
            _user = user;
            _building = building;
            SetStreetInfo();
        }
        private void CommonRoomsPage_Loaded(object sender, RoutedEventArgs e)
        {
            CommonRooms.Clear();
            var commonRooms = _commonRoomRepository.GetByUserAndBuilding(_user.Id, _building.Id);

            foreach (var commonRoom in commonRooms)
                CommonRooms.Add(commonRoom);

            floorsUpDown.Maximum = _building.FloorCount;
        }
        private void LeftArrow_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var homeWindow = Window.GetWindow(this) as HomeWindow;

            if (homeWindow != null)
                homeWindow.HomeFrame.Navigate(new MyBuildingsPage(_user));
        }
        private void SetStreetInfo() => streetInfo.Text = $"{_building.Address.Street.Name} {_building.Address.StreetNumber}";

    }
}
