using CommunityHub.Application.Domain.CommonRooms;
using CommunityHub.Application.Services.CommonRooms;
using CommunityHub.Application.Services.Dtos.CommonRooms;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.Views.Resident.Buildings.CommonRooms.Details;
using System.Collections.ObjectModel;

namespace CommunityHub.Ui.ViewModels.Resident.Buildings.CommonRooms
{
    public class CommonRoomsViewModel : BaseViewModel
    {
        private readonly CommonRoomService _commonRoomService = new();

        public long UserId { get; }
        public long BuildingId { get; }

        public ObservableCollection<CommonRoomDto> CommonRooms { get; } = new();
        public bool HasCommonRooms => CommonRooms.Count > 0;

        public CommonRoomsViewModel(long userId, long buildingId, MainNavigationConsumerPage page) : base(page) {
            UserId = userId;
            BuildingId = buildingId;
        }

        public void OpenCommonRoomDetailsPage(CommonRoomDto commonRoom)
            => Push(new CommonRoomDetailsPage(UserId, commonRoom));

        public void UpdateCommonRooms()
        {
            CommonRooms.Clear();
            var commonRooms = _commonRoomService.GetByBuilding(BuildingId);
            foreach (var commonRoom in commonRooms)
                CommonRooms.Add(commonRoom);

            OnPropertyChanged(nameof(HasCommonRooms));
        }

    }
}
