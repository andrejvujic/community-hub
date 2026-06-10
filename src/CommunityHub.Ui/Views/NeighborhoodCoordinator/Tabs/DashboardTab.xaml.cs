using System.Windows.Controls;
using CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Tabs;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Tabs
{
    public partial class DashboardTab : UserControl
    {
        private readonly DashboardTabViewModel _viewModel;

        public DashboardTab()
        {
            InitializeComponent();
            _viewModel = new DashboardTabViewModel();
            DataContext = _viewModel;
        }

        public void Initialize(long coordinatorId)
        {
            _viewModel.Initialize(coordinatorId);
        }
    }
}
