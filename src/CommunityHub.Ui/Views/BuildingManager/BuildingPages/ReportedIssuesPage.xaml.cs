using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.ReportedIsssue;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using CommunityHub.Application.Database.Repositories;
using System.Diagnostics;


namespace CommunityHub.Ui.Views.BuildingManager.BuildingPages
{
    public partial class ReportedIssuesPage : Page
    {
        private readonly User _user;
        private readonly Building _building;
        public ObservableCollection<ReportedIssue> ReportedIssues = [];
        private readonly ReportedIssueRepository _reportedIssueRepository = new();
        public ReportedIssuesPage(User user, Building building)
        {
            InitializeComponent();
            _user = user;
            _building = building;
            SetStreetInfo();
        }
        private void ReportedIssuesPage_Loaded(object sender, RoutedEventArgs e)
        {
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
    }
}
