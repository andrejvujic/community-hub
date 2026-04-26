using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain.CommonRoom;
using CommunityHub.Application.Domain.CommonRoomRentalRequest;
using CommunityHub.Ui.Helpers.Navigation;
using System.Collections.ObjectModel;

namespace CommunityHub.Ui.Views.Resident.Buildings.CommonRooms.Details.Requests
{
    public partial class CommonRoomRentalRequestsPage : MainNavigatorConsumerPage
    {
        private readonly long _userId;
        private readonly CommonRoomRentalRequestRepository _rentalRequestRepository = new();

        public CommonRoom CommonRoom { get; }
        public ObservableCollection<CommonRoomRentalRequest> RentalRequests { get; } = new();
        public bool HasRentalRequests => RentalRequests.Count > 0;

        public CommonRoomRentalRequestsPage(long userId, CommonRoom commonRoom)
        {
            _userId = userId;
            CommonRoom = commonRoom;

            InitializeComponent();
            DataContext = this;

            UpdateRenalRequests();
        }

        private void UpdateRenalRequests()
        {
            RentalRequests.Clear();
            var rentalRequests =  _rentalRequestRepository.GetByResidentIdAndCommonRoomId(_userId, CommonRoom.Id);
            foreach (var rentalRequest in rentalRequests)
                RentalRequests.Add(rentalRequest);
        }

        private void RequestCommonRoomRentalButton_Click(object sender, System.Windows.RoutedEventArgs e)
            => Push(
                    new RequestCommonRoomRentalPage(
                        _userId,
                        CommonRoom
                    ),
                    onPop: _ => UpdateRenalRequests()
               );
    }
}