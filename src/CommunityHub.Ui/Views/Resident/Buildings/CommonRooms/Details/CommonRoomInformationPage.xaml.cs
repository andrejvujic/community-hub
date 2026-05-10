using CommunityHub.Application.Domain.CommonRooms;
using CommunityHub.Ui.Helpers.Navigation;

namespace CommunityHub.Ui.Views.Resident.Buildings.CommonRooms.Details
{
    public partial class CommonRoomInformationPage : MainNavigatorConsumerPage
    {
        public CommonRoom CommonRoom { get; }

        public CommonRoomInformationPage(CommonRoom commonRoom)
        {
            CommonRoom = commonRoom;
            InitializeComponent();
            DataContext = this;
        }
    }
}
