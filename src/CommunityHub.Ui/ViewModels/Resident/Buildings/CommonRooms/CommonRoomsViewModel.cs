using CommunityHub.Application.Domain.Buildings.CommonRooms;
using CommunityHub.Application.Modules.Buildings.Core.Services.CommonRooms;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CommunityHub.Ui.ViewModels.Resident.Buildings.CommonRooms
{
    public class CommonRoomsViewModel : INotifyPropertyChanged
    {
        private readonly CommonRoomService _commonRoomService = new();

        public long UserId { get; }
        public long BuildingId { get; }

        public ObservableCollection<CommonRoom> CommonRooms { get; } = new();
        public bool HasCommonRooms => CommonRooms.Count > 0;

        public CommonRoomsViewModel(long userId, long buildingId) {
            UserId = userId;
            BuildingId = buildingId;
        }

        public void UpdateCommonRooms()
        {
            CommonRooms.Clear();
            var commonRooms = _commonRoomService.GetByBuilding(BuildingId);
            foreach (var commonRoom in commonRooms)
                CommonRooms.Add(commonRoom);

            OnPropertyChanged(nameof(HasCommonRooms));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
