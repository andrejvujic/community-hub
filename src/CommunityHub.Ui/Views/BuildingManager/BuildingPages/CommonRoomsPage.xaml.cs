using CommunityHub.Application.Domain.Buildings;
using CommunityHub.Application.Domain.Users;
using CommunityHub.Ui.ViewModels.BuildingManager.BuildingPages;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace CommunityHub.Ui.Views.BuildingManager.BuildingPages
{
    public partial class CommonRoomsPage : Page
    {
        private readonly CommonRoomsPageViewModel _viewModel;
        private bool _isSyncingCalendar = false;
        public CommonRoomsPage(User user, Building building)
        {
            InitializeComponent();
            _viewModel = new CommonRoomsPageViewModel(user, building);
            DataContext = _viewModel;

            _viewModel.ReservedDates.CollectionChanged += ReservedDates_CollectionChanged;
            Unloaded += CommonRoomsPage_Unloaded;

            SyncCalendar();
        }
        private void ReservedDates_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            SyncCalendar();
        }
        private void SyncCalendar()
        {
            _isSyncingCalendar = true;

            calendar.SelectedDates.Clear();
            foreach (var date in _viewModel.ReservedDates)
                calendar.SelectedDates.Add(date);

            _isSyncingCalendar = false;
        }
        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if(_isSyncingCalendar) return;

            SyncCalendar();
        }
        private void CommonRoomsPage_Unloaded(object sender, RoutedEventArgs e)
        {
            _viewModel.ReservedDates.CollectionChanged -= ReservedDates_CollectionChanged;
            Unloaded -= CommonRoomsPage_Unloaded;
        }
    }
}