using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using CommunityHub.Application.Domain.Buildings;
using CommunityHub.Application.Database.Repositories.Issues;
using CommunityHub.Application.Domain.Issues;
using CommunityHub.Application.Domain.Users;

namespace CommunityHub.Ui.Views.BuildingManager.BuildingPages
{
    public partial class ReportedIssuesPage : Page
    {
        private readonly User _user;
        private readonly Building _building;
        public ObservableCollection<Issue> ReportedIssues { get; set; } = [];
        private readonly IssueRepository _reportedIssueRepository = new();
        public ReportedIssuesPage(User user, Building building)
        {
            InitializeComponent();
            _user = user;
            _building = building;
            DataContext = this;
            SetStreetInfo();
        }
        private void ReportedIssuesPage_Loaded(object sender, RoutedEventArgs e) => RefreshReportedIssues();
        private void RefreshReportedIssues()
        {
            ReportedIssues.Clear();

            var reportedIssues = LoadReportedIssues();
            foreach (var reportedIssue in reportedIssues)
                ReportedIssues.Add(reportedIssue);
        }
        private void LeftArrow_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var homeWindow = Window.GetWindow(this) as HomeWindow;

            if (homeWindow != null)
                homeWindow.HomeFrame.Navigate(new MyBuildingsPage(_user));
        }
        private void SetStreetInfo() => streetInfo.Text = $"{_building.Address.Street.Name} {_building.Address.StreetNumber}";
        private List<Issue> LoadReportedIssues()
        {
            return _reportedIssueRepository.GetAllByBuilding(_building.Id);
        }
        private void SolveIssue_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Da li želite da označite ovaj problem kao rešen?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.No) return;

            var reportedIssue = (sender as Grid)?.DataContext as Issue;

            if (reportedIssue == null) return;
            
            _reportedIssueRepository.UpdateStatus(reportedIssue.Id, IssueStatus.PotentiallyResolved);

            RefreshReportedIssues();
        }
    }
}
