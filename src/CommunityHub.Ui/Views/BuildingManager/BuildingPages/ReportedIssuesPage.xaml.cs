using System.Windows;
using System.Windows.Controls;
using CommunityHub.Application.Domain.Buildings;
using System.Windows.Input;
using CommunityHub.Application.Domain.Users;
using CommunityHub.Ui.ViewModels.BuildingManager.BuildingPages;

namespace CommunityHub.Ui.Views.BuildingManager.BuildingPages
{
    public partial class ReportedIssuesPage : Page
    {
        private readonly ReportedIssuesPageViewModel _viewModel;
        public ReportedIssuesPage(User user, Building building)
        {
            InitializeComponent();
            _viewModel = new ReportedIssuesPageViewModel(user, building);
            DataContext = _viewModel;
        }
    }
}