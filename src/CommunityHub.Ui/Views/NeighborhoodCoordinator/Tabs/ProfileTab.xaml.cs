using System.Windows.Controls;
using CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Tabs;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Tabs
{
    public partial class ProfileTab : UserControl
    {
        private ProfileTabViewModel _viewModel = null!;

        public ProfileTab()
        {
            InitializeComponent();
        }

        public void Initialize(long coordinatorId)
        {
            _viewModel = new ProfileTabViewModel(coordinatorId);
            DataContext = _viewModel;
            _viewModel.LoadData();
        }
    }
}