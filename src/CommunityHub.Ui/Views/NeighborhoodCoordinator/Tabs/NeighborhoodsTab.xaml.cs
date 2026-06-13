using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Tabs;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Tabs
{
    public partial class NeighborhoodsTab : UserControl
    {
        private readonly NeighborhoodsTabViewModel _viewModel;

        public NeighborhoodsTab()
        {
            InitializeComponent();

            _viewModel = new NeighborhoodsTabViewModel();
            DataContext = _viewModel;

            this.IsVisibleChanged += NeighborhoodsTab_IsVisibleChanged;
        }

        public void Initialize(long coordinatorId)
        {
            _viewModel.Initialize(coordinatorId);

            Window.GetWindow(this).KeyDown += NeighborhoodsTab_KeyDown;
        }

        private void NeighborhoodsTab_KeyDown(object sender, KeyEventArgs e)
        {
            if (!IsVisible) return;

            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                && e.Key == Key.N)
            {
                if (_viewModel.RegisterCommand.CanExecute(null))
                {
                    _viewModel.RegisterCommand.Execute(null);
                }
            }
        }

        private void NeighborhoodsTab_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
                _viewModel.LoadNeighborhoods();
        }
    }
}