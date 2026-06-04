using CommunityHub.Application.Domain.CommonRooms;
using CommunityHub.Application.Services.Dtos.CommonRooms;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.Views.Resident.Buildings.CommonRooms.Details;
using CommunityHub.Ui.Views.Resident.Buildings.CommonRooms.Details.Requests;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.CommonRooms
{
    public partial class CommonRoomDetailsPage : MainNavigationConsumerPage
    {
        private readonly long _userId;

        public CommonRoomDto CommonRoom { get; }

        public CommonRoomDetailsPage(long userId, CommonRoomDto commonRoom)
        {
            _userId = userId;
            CommonRoom = commonRoom;

            InitializeComponent();
            DataContext = this;

            OpenCommonRoomInformationPage();
        }

        private void OpenCommonRoomInformationPage()
            => LocalFrame.Navigate(new CommonRoomInformationPage(CommonRoom));

        private void OpenCommonRoomRentalRequestsPage()
            => LocalFrame.Navigate(new CommonRoomRentalRequestsPage(_userId, CommonRoom));

        private void BackButton_Click(object sender, MouseButtonEventArgs e) => Pop();

        private void BottomBar_InformationItemClick(object sender, MouseButtonEventArgs e)
            => OpenCommonRoomInformationPage();

        private void BottomBar_RentalItemClick(object sender, MouseButtonEventArgs e)
        => OpenCommonRoomRentalRequestsPage();
    }
}
