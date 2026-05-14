using System.Windows;
using System.Windows.Controls;
using CommunityHub.Application.Domain.Buildings;
using System.Windows.Input;
using CommunityHub.Application.Domain.Users;
using CommunityHub.Ui.ViewModels.BuildingManager.BuildingPages;
using CommunityHub.Application.Services.Dtos.Issues;

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
        private void LeftArrow_Click(object sender, MouseButtonEventArgs e)
        {
            var homeWindow = Window.GetWindow(this) as HomeWindow;

            if (homeWindow != null)
                homeWindow.HomeFrame.Navigate(new MyBuildingsPage(_viewModel.User));
        }
        private void SolveIssue_Click(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Da li želite da označite ovaj problem kao rešen?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.No) return;

            var reportedIssue = (sender as Grid)?.DataContext as IssueDto;

            if (reportedIssue == null) return;
            
            _viewModel.MarkIssueAsResolved(reportedIssue);
        }
    }
}