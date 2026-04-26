using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.Views.Resident.Buildings.CommonRooms;
using CommunityHub.Ui.Views.Resident.Buildings.NoticeBoard;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings
{
    public partial class BuildingOptionsPage : MainNavigatorConsumerPage
    {
        private readonly long _userId;

        public Building Building { get; }
        public BuildingOptionsPage(long userId, Building building)
        {
            InitializeComponent();
            DataContext = this;

            _userId = userId;
            Building = building;
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e) => Pop();

        private void OpenBuildingDetailsPageButton_Click(object sender, MouseButtonEventArgs e)
            => Push(new BuildingDetailsPage(Building));

        private void OpenNoticeBoardPageButton_Click(object sender, MouseButtonEventArgs e)
            => Push(new NoticeBoardPage(
                userId: _userId,
                buildingId: Building.Id
            ));

        private void OpenPersonalAdsPageButton_Click(object sender, MouseButtonEventArgs e)
            => Push(new PersonalAdsPage(
                userId: _userId,
                buildingId: Building.Id
            ));

        private void OpenCommonRoomsPageButton_Click(object sender, MouseButtonEventArgs e)
            => Push(new CommonRoomsPage(
                userId: _userId,
                buildingId: Building.Id
            ));
    }
}
