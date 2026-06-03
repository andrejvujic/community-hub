using System.Windows.Controls;
using CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Tabs;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Tabs
{
    public partial class RequestsTab : UserControl
    {
        private RequestsTabViewModel _viewModel = null!;

        public RequestsTab()
        {
            InitializeComponent();
        }

        public void Initialize(long coordinatorId)
        {
            _viewModel = new RequestsTabViewModel(coordinatorId);
            DataContext = _viewModel;
            _viewModel.LoadRequests(null);
        }
    }
}