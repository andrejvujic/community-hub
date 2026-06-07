using System.Windows;
using System.Windows.Controls;
using CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Tabs;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Tabs
{
    public partial class ProfileTab : UserControl
    {
        private readonly ProfileTabViewModel _viewModel;

        public ProfileTab()
        {
            InitializeComponent();

            _viewModel = new ProfileTabViewModel();
            DataContext = _viewModel;

            this.IsVisibleChanged += ProfileTab_IsVisibleChanged;
        }

        public void Initialize(long coordinatorId)
        {
            _viewModel.Initialize(coordinatorId);
        }

        private void ProfileTab_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                _viewModel?.LoadData();
            }
        }
    }
}