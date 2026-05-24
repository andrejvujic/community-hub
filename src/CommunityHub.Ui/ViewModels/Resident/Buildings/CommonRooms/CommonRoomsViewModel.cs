using CommunityHub.Application.Services.CommonRooms;
using CommunityHub.Application.Services.Dtos.CommonRooms;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.Views.Resident.Buildings.CommonRooms;
using CommunityHub.Ui.Views.Resident.Buildings.CommonRooms.Details;
using System.Collections.ObjectModel;

namespace CommunityHub.Ui.ViewModels.Resident.Buildings.CommonRooms
{
    public class CommonRoomsViewModel : BaseViewModel
    {
        private readonly CommonRoomService _commonRoomService = new();

        private readonly long _userId;
        private readonly long _buildingId;

        public ObservableCollection<CommonRoomDto> CommonRooms { get; } = new();
        public bool HasCommonRooms => CommonRooms.Count > 0;

        public CommonRoomsViewModel(long userId, long buildingId, IMainNavigationConsumer navigationConsumer) : base(navigationConsumer) {
            _userId = userId;
            _buildingId = buildingId;
            OpenCommonRoomDetailsCommand = new(OpenCommonRoomDetailsPage);
        }

        public RelayCommand<CommonRoomDto> OpenCommonRoomDetailsCommand { get; }

        private void OpenCommonRoomDetailsPage(CommonRoomDto? commonRoom)
        {
            if (commonRoom is null) return;
            Push(new CommonRoomDetailsPage(_userId, commonRoom));
        }

        public void UpdateCommonRooms()
        {
            CommonRooms.Clear();
            var commonRooms = _commonRoomService.GetByBuilding(_buildingId);
            foreach (var commonRoom in commonRooms)
                CommonRooms.Add(commonRoom);

            OnPropertyChanged(nameof(HasCommonRooms));
        }

    }
}
