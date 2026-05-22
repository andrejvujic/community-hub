using CommunityHub.Application.Domain.CommonRooms;
using CommunityHub.Application.Services.Dtos.CommonRooms;
using CommunityHub.Ui.Helpers.Navigation;

namespace CommunityHub.Ui.Views.Resident.Buildings.CommonRooms.Details
{
    public partial class CommonRoomInformationPage : MainNavigatorConsumerPage
    {
        public CommonRoomDto CommonRoom { get; }

        public CommonRoomInformationPage(CommonRoomDto commonRoom)
        {
            CommonRoom = commonRoom;
            InitializeComponent();
            DataContext = this;
        }
    }
}
