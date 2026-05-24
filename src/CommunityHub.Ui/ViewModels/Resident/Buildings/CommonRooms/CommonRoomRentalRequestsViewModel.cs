using CommunityHub.Application.Domain.CommonRooms.RentalRequests;
using CommunityHub.Application.Services.CommonRooms;
using CommunityHub.Application.Services.Dtos.CommonRooms;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.Views.Resident.Buildings.CommonRooms.Details.Requests;
using System.Collections.ObjectModel;

namespace CommunityHub.Ui.ViewModels.Resident.Buildings.CommonRooms
{
    public class CommonRoomRentalRequestsViewModel: BaseViewModel
    {
        private readonly CommonRoomRentalRequestService _rentalRequestService = new();

        private readonly long _userId;
        public CommonRoomDto CommonRoom { get; }

        public CommonRoomRentalRequestsViewModel(
            long userId, 
            CommonRoomDto commonRoom, 
            IMainNavigationConsumer navigationConsumer
        ) : base(navigationConsumer)
        {
            _userId = userId;
            CommonRoom = commonRoom;

            OpenRequestCommonRoomRentalCommand = new(OpenRequestCommonRoomRentalPage);
            OpenRentalRequestDetailsCommand = new(OpenRentalRequestDetailsPage);
        }

        public RelayCommand OpenRequestCommonRoomRentalCommand { get; }
        public RelayCommand<CommonRoomRentalRequestDto> OpenRentalRequestDetailsCommand { get; }

        public ObservableCollection<CommonRoomRentalRequestDto> RentalRequests { get; } = new();
        public bool HasRentalRequests => RentalRequests.Count > 0;

        private void OpenRequestCommonRoomRentalPage()
            => Push(
                new RequestCommonRoomRentalPage(_userId, CommonRoom),
                onPop: _ => UpdateRenalRequests()
            );

        private void OpenRentalRequestDetailsPage(CommonRoomRentalRequestDto? request)
        {
            if (request is null) return;
            Push(
                new CommonRoomRentalRequestDetailsPage(request),
                onPop: _ => UpdateRenalRequests()
            );
        }

        public void UpdateRenalRequests()
        {
            var rentalRequests = _rentalRequestService.GetByResidentAndCommonRoom(
                _userId,
                CommonRoom.Id
            );

            RentalRequests.Clear();
            foreach (var rentalRequest in rentalRequests)
                RentalRequests.Add(rentalRequest);

            OnPropertyChanged(nameof(HasRentalRequests));
        }

    }
}
