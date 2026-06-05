using System.Windows;
using CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Windows;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Windows
{
    public partial class InfoDialog : Window
    {
        private readonly InfoDialogViewModel _viewModel;

        public InfoDialog(string title, string message, Window owner)
        {
            InitializeComponent();
            Owner = owner;

            _viewModel = new InfoDialogViewModel(title, message);
            DataContext = _viewModel;
        }
    }
}