using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.Views.Resident.Buildings.CommonRooms;
using System.Windows.Input;
using CommunityHub.Ui.Views.Resident.Buildings.Ads.Browse.OtherAds;
using CommunityHub.Ui.Views.Resident.Buildings.Ads.Browse.MyAds;
using CommunityHub.Ui.Views.Resident.Buildings.Issues;
using CommunityHub.Ui.Views.Resident.Buildings.Assemblies;
using CommunityHub.Application.Services.Dtos.Buildings;

namespace CommunityHub.Ui.Views.Resident.Buildings.Browse
{
    public partial class BuildingOptionsPage : MainNavigationConsumerPage
    {
        private readonly long _userId;

        public BuildingDto Building { get; }
        public BuildingOptionsPage(long userId, BuildingDto building)
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
            => Push(new AdsPage(
                userId: _userId,
                buildingId: Building.Id
            ));

        private void OpenPersonalAdsPageButton_Click(object sender, MouseButtonEventArgs e)
            => Push(new MyAdsPage(
                userId: _userId,
                buildingId: Building.Id
            ));

        private void OpenCommonRoomsPageButton_Click(object sender, MouseButtonEventArgs e)
            => Push(new CommonRoomsPage(
                userId: _userId,
                buildingId: Building.Id
            ));

        private void OpenIssuesPageButton_Click(object sender, MouseButtonEventArgs e)
            => Push(new IssuesPage(
                userId: _userId,
                buildingId: Building.Id 
            ));

        private void OpenAssembliesPageButton_Click(object sender, MouseButtonEventArgs e)
            => Push(new AssembliesPage(
                residentId: _userId,
                buildingId: Building.Id
            ));
    }
}
