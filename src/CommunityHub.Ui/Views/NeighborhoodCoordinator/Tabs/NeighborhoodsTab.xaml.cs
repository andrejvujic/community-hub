using System.Windows;
using System.Windows.Controls;
using CommunityHub.Application.Services.Dtos.Neighborhoods;
using CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Tabs;
using CommunityHub.Ui.Views.NeighborhoodCoordinator.Windows;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Tabs
{
    public partial class NeighborhoodsTab : UserControl
    {
        private NeighborhoodsTabViewModel _viewModel = null!;
        private long _coordinatorId;

        public NeighborhoodsTab()
        {
            InitializeComponent();
            this.IsVisibleChanged += NeighborhoodsTab_IsVisibleChanged;
        }

        public void Initialize(long coordinatorId)
        {
            _coordinatorId = coordinatorId;
            _viewModel = new NeighborhoodsTabViewModel(coordinatorId);
            DataContext = _viewModel;
            _viewModel.LoadNeighborhoods();
        }

        private void NeighborhoodsTab_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
                _viewModel?.LoadNeighborhoods();
        }

        private void NeighborhoodsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NeighborhoodsListBox.SelectedItem is NeighborhoodDto selected)
                _viewModel.SelectedNeighborhood = selected;
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new RegisterNeighborhoodWindow(_coordinatorId);
            window.Owner = Window.GetWindow(this);

            if (window.ShowDialog() == true)
                _viewModel.LoadNeighborhoods();
        }
    }
}
