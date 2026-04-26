using CommunityHub.Application.Domain.CommonRoom;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.Views.Resident.Buildings.CommonRooms.Details;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.CommonRooms
{
    public partial class CommonRoomDetailsPage : MainNavigatorConsumerPage
    {
        private readonly long _userId;

        public CommonRoom CommonRoom { get; }

        public CommonRoomDetailsPage(long userId, CommonRoom commonRoom)
        {
            _userId = userId;
            CommonRoom = commonRoom;

            InitializeComponent();
            DataContext = this;

            OpenCommonRoomInformationPage();
        }

        private void OpenCommonRoomInformationPage()
            => LocalFrame.Navigate(new CommonRoomInformationPage(CommonRoom));

        private void BackButton_Click(object sender, MouseButtonEventArgs e) => Pop();

        private void BottomBar_InformationItemClick(object sender, MouseButtonEventArgs e)
            => OpenCommonRoomInformationPage();
    }
}
