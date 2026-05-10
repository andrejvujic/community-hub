using CommunityHub.Application.Domain.CommonRooms;
using CommunityHub.Application.Domain.CommonRooms.RentalRequests;
using CommunityHub.Application.Services.CommonRooms;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CommunityHub.Ui.ViewModels.Resident.Buildings.CommonRooms
{
    public class CommonRoomRentalRequestsViewModel: INotifyPropertyChanged
    {
        private readonly CommonRoomRentalRequestService _rentalRequestService = new();

        public long UserId { get; }
        public CommonRoom CommonRoom { get; }

        public CommonRoomRentalRequestsViewModel(long userId, CommonRoom commonRoom)
        {
            UserId = userId;
            CommonRoom = commonRoom;
        }
        public ObservableCollection<CommonRoomRentalRequest> RentalRequests { get; } = new();
        public bool HasRentalRequests => RentalRequests.Count > 0;


        public void UpdateRenalRequests()
        {
            var rentalRequests = _rentalRequestService.GetByResidentAndCommonRoom(
                UserId,
                CommonRoom.Id
            );

            RentalRequests.Clear();
            foreach (var rentalRequest in rentalRequests)
                RentalRequests.Add(rentalRequest);

            OnPropertyChanged(nameof(HasRentalRequests));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
