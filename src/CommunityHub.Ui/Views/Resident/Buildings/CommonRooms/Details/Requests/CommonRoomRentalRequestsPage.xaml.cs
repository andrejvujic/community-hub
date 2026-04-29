using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain.Buildings.CommonRooms;
using CommunityHub.Application.Domain.Buildings.CommonRooms.RentalRequests;
using CommunityHub.Ui.Helpers.Navigation;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.CommonRooms.Details.Requests
{
    public partial class CommonRoomRentalRequestsPage : MainNavigatorConsumerPage, INotifyPropertyChanged
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
            var rentalRequests =  _rentalRequestRepository.GetByResidentAndCommonRoom(_userId, CommonRoom.Id);
            foreach (var rentalRequest in rentalRequests)
                RentalRequests.Add(rentalRequest);

            OnPropertyChanged(nameof(HasRentalRequests));
        }

        private void RequestCommonRoomRentalButton_Click(object sender, RoutedEventArgs e)
            => Push(
                    new RequestCommonRoomRentalPage(
                        _userId,
                        CommonRoom
                    ),
                    onPop: _ => UpdateRenalRequests()
               );

        private void RentalRequestItem_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is not FrameworkElement element) return;

            if (element.DataContext is not CommonRoomRentalRequest request) return;

            Push(
                new CommonRoomRentalRequestDetailsPage(request),
                onPop: _ => UpdateRenalRequests()
            );
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}