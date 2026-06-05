using System.Windows;
using CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Windows;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Windows
{
    public partial class RejectRequestWindow : Window
    {
        private readonly RejectRequestWindowViewModel _viewModel;

        public string Comment => _viewModel.Comment;

        public RejectRequestWindow(string ime, string prezime, string kvart)
        {
            InitializeComponent();

            _viewModel = new RejectRequestWindowViewModel(ime, prezime, kvart);
            DataContext = _viewModel;
        }
    }
}