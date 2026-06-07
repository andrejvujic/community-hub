using CommunityHub.Ui.ViewModels.Citizen.Neighborhoods;
using CommunityHub.Ui.Views.Citizen.Neighborhoods.CoordinatorReviews;
using System.Windows;
using System.Windows.Controls;

namespace CommunityHub.Ui.Views.Citizen.Neighborhoods
{
    public partial class NeighborhoodsPage : Page
    {
        private NeighborhoodsViewModel? _viewModel;
        private readonly long _userId;

        public NeighborhoodsPage(long userId)
        {
            InitializeComponent();
            _userId = userId;

            Loaded += (_, _) =>
            {
                _viewModel = new NeighborhoodsViewModel(userId);
                _viewModel.SearchRequested += (_, _) => HandleSearch();
                _viewModel.RequestAccessRequested += (_, _) => HandleRequestAccess();
                _viewModel.CoordinatorRequested += (_, _) => HandleCoordinator();
                _viewModel.MyAccessRequestsRequested += (_, _) => HandleMyAccessRequests();
                DataContext = _viewModel;
            };
        }

        private void HandleSearch()
        {
            var window = new SearchCriteriaWindow(_viewModel?.LastSearchParameters);
            if (window.ShowDialog() == true)
            {
                _viewModel?.PerformSearch(window.Result);
            }
        }

        private void HandleRequestAccess()
        {
            var dlg = new RequestNeighborhoodAccessWindow(_viewModel!.SelectedNeighborhood!.Name ?? string.Empty);
            dlg.Owner = Window.GetWindow(this);
            if (dlg.ShowDialog() != true || !dlg.Confirmed) return;

            if (_viewModel.RequestNeighborhoodAccess(_viewModel.SelectedNeighborhood))
            {
                var message = _viewModel.GetAccessResultMessage(_viewModel.SelectedNeighborhood);
                MessageBox.Show(message, "Informacija", MessageBoxButton.OK, MessageBoxImage.Information);
                _viewModel.LoadNeighborhoods();
            }
        }

        private void HandleCoordinator()
        {
            var window = new MyNeighborhoodCoordinatorWindow(_userId);
            window.Owner = Window.GetWindow(this);
            window.ShowDialog();
        }

        private void HandleMyAccessRequests()
        {
            var wnd = new MyAccessRequestsWindow(_userId);
            wnd.Owner = Window.GetWindow(this);
            wnd.ShowDialog();
        }
    }
}
