using System.Windows;
using CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Windows;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Windows
{
    public partial class ConfirmDialog : Window
    {
        private readonly ConfirmDialogViewModel _viewModel;

        public ConfirmDialog(string title, string message, Window owner)
        {
            InitializeComponent();
            Owner = owner;

            _viewModel = new ConfirmDialogViewModel(title, message);
            DataContext = _viewModel;
        }
    }
}
