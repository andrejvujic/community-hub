using System.Windows.Controls;
using CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Tabs;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Tabs
{
    public partial class RequestsTab : UserControl
    {
        private readonly RequestsTabViewModel _viewModel;

        public RequestsTab()
        {
            InitializeComponent();

            _viewModel = new RequestsTabViewModel();
            DataContext = _viewModel;
        }

        public void Initialize(long coordinatorId)
        {
            _viewModel.Initialize(coordinatorId);
        }
    }
}