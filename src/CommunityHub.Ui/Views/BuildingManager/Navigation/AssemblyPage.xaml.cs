using System.Windows.Controls;
using CommunityHub.Application.Domain.Users;
using CommunityHub.Ui.ViewModels.BuildingManager.Navigation;

namespace CommunityHub.Ui.Views.BuildingManager
{
    public partial class AssemblyPage : Page
    {
        private readonly AssemblyPageViewModel _viewModel;
        public AssemblyPage(User user)
        {
            InitializeComponent();
            _viewModel = new AssemblyPageViewModel(user);
            DataContext = _viewModel;
        }
    }
}