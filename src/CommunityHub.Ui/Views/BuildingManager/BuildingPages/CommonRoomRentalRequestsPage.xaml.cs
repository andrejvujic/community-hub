using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.CommonRoom;
using CommunityHub.Application.Domain.CommonRoomRentalRequest;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CommunityHub.Ui.Views.BuildingManager.BuildingPages
{
    public partial class CommonRoomRentalRequestsPage : Page
    {
        private readonly User _user;
        private readonly Building _building;
        private Border? _selectedCard;
        private readonly CommonRoomRentalRequestRepository _commonRoomRentalRequestRepository = new CommonRoomRentalRequestRepository();
        public ObservableCollection<CommonRoomRentalRequest> RentalRequests { get; set; } = [];
        public CommonRoomRentalRequestsPage(User user, Building building)
        {
            InitializeComponent();
            _user = user;
            _building = building;
            SetStreetInfo();
            DataContext = this;
        }
        private void CommonRoomRentalRequestsPage_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshRentalRequests();
        }
        private void LeftArrow_Click(object sender, MouseButtonEventArgs e)
        {
            var homeWindow = Window.GetWindow(this) as HomeWindow;

            if (homeWindow != null)
                homeWindow.HomeFrame.Navigate(new MyBuildingsPage(_user));
        }
        private void SetStreetInfo() => streetInfo.Text = $"{_building.Address.Street.Name} {_building.Address.StreetNumber}";
        private void RefreshRentalRequests()
        {
            RentalRequests.Clear();
            var rentalRequests = _commonRoomRentalRequestRepository.GetAllByBuildingId(_building.Id);

            foreach (var request in rentalRequests)
                RentalRequests.Add(request);
        }
        public void CheckAndUpdateRentalDateStatuses()
        {
            var rentalRequests = _commonRoomRentalRequestRepository.GetAllByBuildingId(_building.Id);

            foreach (var request in rentalRequests)
            {
                if (request.Status != RentalRequestStatus.Waiting)
                    continue;

                var type = request.CommonRoom.RentalType;

                switch (type)
                {
                    case RentalType.OneDay:
                        UpdateOneDayRequestStatus(request);
                        break;
                    case RentalType.MultiDay:
                        UpdateMultiDayRequestStatus(request);
                        break;
                }
            }
        }
        private void UpdateMultiDayRequestStatus(CommonRoomRentalRequest request)
        {
            var reservedDates = _commonRoomRentalRequestRepository.GetReservedDates(request.CommonRoom.Id, _building.Id);

            bool hasConflict = reservedDates.Any(d => d >= request.RequestedStartDate && d <= request.RequestedEndDate);

            if (!hasConflict)
                _commonRoomRentalRequestRepository.UpdateStatusAndAssignedDate(request.Id, RentalRequestStatus.Approved, null);
        }
        private void UpdateOneDayRequestStatus(CommonRoomRentalRequest request)
        {
            var reservedDates = _commonRoomRentalRequestRepository.GetReservedDates(request.CommonRoom.Id, _building.Id);
            bool isAllReserved = true;

            DateTime current = request.RequestedStartDate;
            while (current <= request.RequestedEndDate)
            {
                if (!reservedDates.Contains(current))
                {
                    isAllReserved = false;
                    break;
                }
                current = current.AddDays(1);
            }

            if (isAllReserved)
                _commonRoomRentalRequestRepository.UpdateStatusAndAssignedDate(request.Id, RentalRequestStatus.Rejected, null);
        }
        private void RentalRequestCard_Click(object sender, MouseButtonEventArgs e)
        {
            SetCardBackground(sender);

            var border = sender as Border;
            CommonRoomRentalRequest? rentalRequest = border?.DataContext as CommonRoomRentalRequest;

            ShowManagerAction(rentalRequest);
        }
        private void SetCardBackground(object sender)
        {
            if (_selectedCard != null)
                _selectedCard.Background = new SolidColorBrush(Colors.White);

            _selectedCard = sender as Border;
            if (_selectedCard != null)
                _selectedCard.Background = new SolidColorBrush(Colors.LightGray);
        }
        private void ShowManagerAction(CommonRoomRentalRequest? request)
        {
            SetVisibility();

            switch (request?.Status)
            {
                case RentalRequestStatus.Approved:
                    approvedActionText.Visibility = Visibility.Visible;
                    break;
                case RentalRequestStatus.Rejected:
                    rejectedActionText.Visibility = Visibility.Visible;
                    break;
                case RentalRequestStatus.WaitingDateChange:
                    waitingDateChangeActionText.Visibility = Visibility.Visible;
                    break;
                case RentalRequestStatus.Waiting:
                    HandleWaitingStatus(request);
                    break;
            }
        }
        private void SetVisibility()
        {
            defaultActionText.Visibility = Visibility.Collapsed;
            approvedActionText.Visibility = Visibility.Collapsed;
            rejectedActionText.Visibility = Visibility.Collapsed;
            waitingDateChangeActionText.Visibility = Visibility.Collapsed;
            oneDayPanel.Visibility = Visibility.Collapsed;
            multiDayPanel.Visibility = Visibility.Collapsed;
        }
        private void HandleWaitingStatus(CommonRoomRentalRequest request)
        {
            if (request.CommonRoom.RentalType == RentalType.OneDay)
                SuggestOneDayDates(request);
            else
                SuggestMultiDayDates(request);
        }
        private void SuggestOneDayDates(CommonRoomRentalRequest request)
        {
            var reservedDates = _commonRoomRentalRequestRepository.GetReservedDates(request.CommonRoom.Id, _building.Id);
            freeDaysPanel.Children.Clear();
            oneDayPanel.Visibility = Visibility.Visible;

            DateTime current = request.RequestedStartDate;

            while(current <= request.RequestedEndDate)
            {
                if(!reservedDates.Contains(current))
                {
                    var day = current;
                    var button = CreateOneDayButton(day);

                    button.Click += (s, e) => FreeDayButton_Click(s, e, request);
                    freeDaysPanel.Children.Add(button);
                }

                current = current.AddDays(1);
            }
        }
        private Button CreateOneDayButton(DateTime day)
        {
            return new Button
            {
                Content = day.ToString("dd.MM.yyyy."),
                Tag = day,
                Margin = new Thickness(0, 6, 0, 6),
                Padding = new Thickness(10, 6, 10, 6),
                FontSize = 15,
                Cursor = Cursors.Hand
            };
        }
        private void FreeDayButton_Click(object sender, RoutedEventArgs e, CommonRoomRentalRequest request)
        {
            var result = MessageBox.Show("Da li želite da promenite datum rezervacije?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.No) return;

            var button = sender as Button;
            var day = (DateTime)button!.Tag;

            _commonRoomRentalRequestRepository.UpdateStatusAndAssignedDate(request.Id, RentalRequestStatus.Approved, day);
            CheckAndUpdateRentalDateStatuses();
            RefreshRentalRequests();
            oneDayPanel.Visibility = Visibility.Collapsed;

        }
        private void SuggestMultiDayDates(CommonRoomRentalRequest request)
        {
            var reservedDates = _commonRoomRentalRequestRepository.GetReservedDates(request.CommonRoom.Id, _building.Id);
            suggestedRangesPanel.Children.Clear();
            multiDayPanel.Visibility = Visibility.Visible;

            int duration = (request.RequestedEndDate - request.RequestedStartDate).Days + 1;
            int foundCount = 0;
            DateTime candidateStart = request.RequestedStartDate.AddDays(1);

            while (foundCount < 3)
            {
                DateTime candidateEnd = candidateStart.AddDays(duration - 1);
                bool isFree = true;
                DateTime day = candidateStart;

                while(day <= candidateEnd)
                {
                    if (reservedDates.Contains(day))
                    {
                        isFree = false;
                        break;
                    }
                    day = day.AddDays(1);
                }

                if(isFree)
                {
                    var proposedStart = candidateStart;
                    var proposedEnd = candidateEnd;
                    var button = CreateMultiDayButton(proposedStart, proposedEnd);

                    button.Click += (s, e) => FreeRangeButton_Click(s, e, request);
                    suggestedRangesPanel.Children.Add(button);

                    foundCount++;
                }

                candidateStart = candidateStart.AddDays(1);
            }
        }
        private Button CreateMultiDayButton(DateTime proposedStart, DateTime proposedEnd)
        {
            return new Button
            {
                Content = $"{proposedStart:dd.MM.yyyy.} - {proposedEnd:dd.MM.yyyy.}",
                Tag = (proposedStart, proposedEnd),
                Margin = new Thickness(0, 6, 0, 6),
                Padding = new Thickness(10, 6, 10, 6),
                FontSize = 15,
                Cursor = Cursors.Hand
            };
        }
        private void FreeRangeButton_Click(object sender, RoutedEventArgs e, CommonRoomRentalRequest request)
        {
            var result = MessageBox.Show("Da li želite da promenite opseg rezervacije", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.No) return;

            var button = sender as Button;
            var (proposedStartDate, proposedEndDate) = ((DateTime, DateTime))button!.Tag;

            _commonRoomRentalRequestRepository.UpdateStatusAndRange(request.Id, RentalRequestStatus.WaitingDateChange, proposedStartDate, proposedEndDate);
            CheckAndUpdateRentalDateStatuses();
            RefreshRentalRequests();
            multiDayPanel.Visibility = Visibility.Collapsed;
        }
    }
}