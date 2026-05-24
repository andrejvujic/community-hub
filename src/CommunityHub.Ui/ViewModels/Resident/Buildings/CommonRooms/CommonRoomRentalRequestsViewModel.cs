using CommunityHub.Application.Domain.CommonRooms.RentalRequests;
using CommunityHub.Application.Services.CommonRooms;
using CommunityHub.Application.Services.Dtos.CommonRooms;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.Views.Resident.Buildings.CommonRooms.Details.Requests;
using System.Collections.ObjectModel;

namespace CommunityHub.Ui.ViewModels.Resident.Buildings.CommonRooms
{
    public class CommonRoomRentalRequestsViewModel: BaseViewModel
    {
        private readonly CommonRoomRentalRequestService _rentalRequestService = new();

        public long UserId { get; }
        public CommonRoomDto CommonRoom { get; }

        public CommonRoomRentalRequestsViewModel(long userId, CommonRoomDto commonRoom, MainNavigationConsumerPage page) : base(page)
        {
            UserId = userId;
            CommonRoom = commonRoom;
        }

        public void OpenRequestCommonRoomRentalPage()
            => Push(
                new RequestCommonRoomRentalPage(UserId, CommonRoom),
                onPop: _ => UpdateRenalRequests()
            );

        public void OpenRentalRequestDetailsPage(CommonRoomRentalRequestDto request)
            => Push(
                new CommonRoomRentalRequestDetailsPage(request),
                onPop: _ => UpdateRenalRequests()
            );
        public ObservableCollection<CommonRoomRentalRequestDto> RentalRequests { get; } = new();
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

    }
}
