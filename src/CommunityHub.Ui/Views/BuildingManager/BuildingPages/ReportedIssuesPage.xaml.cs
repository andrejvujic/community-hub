using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.ReportedIsssue;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using CommunityHub.Application.Database.Repositories;

namespace CommunityHub.Ui.Views.BuildingManager.BuildingPages
{
    public partial class ReportedIssuesPage : Page
    {
        private readonly User _user;
        private readonly Building _building;
        public ObservableCollection<ReportedIssue> ReportedIssues { get; set; } = [];
        private readonly ReportedIssueRepository _reportedIssueRepository = new();
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
        private List<ReportedIssue> LoadReportedIssues()
        {
            return _reportedIssueRepository.GetAllByBuilding(_building.Id);
        }
        private void SolveIssue_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Da li želite da označite ovaj problem kao rešen?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.No) return;

            var reportedIssue = (sender as Grid)?.DataContext as ReportedIssue;

            if (reportedIssue == null) return;
            
            _reportedIssueRepository.UpdateStatus(reportedIssue.Id, ReportedIssueStatus.PotentiallyResolved);

            RefreshReportedIssues();
        }
    }
}
