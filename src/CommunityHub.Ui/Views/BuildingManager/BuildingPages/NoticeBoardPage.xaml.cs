using CommunityHub.Application.Domain.Buildings;
using CommunityHub.Application.Domain.Users;
using CommunityHub.Ui.ViewModels.BuildingManager.BuildingPages;
using System.Windows;
using System.Windows.Controls;

namespace CommunityHub.Ui.Views.BuildingManager.BuildingPages
{
    public partial class NoticeBoardPage : Page
    {
        private readonly NoticeBoardPageViewModel _viewModel;
        public NoticeBoardPage(User user, Building building)
        {
            InitializeComponent();
            _viewModel = new NoticeBoardPageViewModel(user, building);
            DataContext = _viewModel;
        }
    }
}