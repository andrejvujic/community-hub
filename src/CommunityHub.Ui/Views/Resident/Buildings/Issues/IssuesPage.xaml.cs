using CommunityHub.Application.Database.Repositories.Issues;
using CommunityHub.Application.Domain.Issues;
using CommunityHub.Ui.Helpers.Navigation;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.Issues
{
    public partial class IssuesPage : MainNavigatorConsumerPage, INotifyPropertyChanged
    {
        private readonly long _userId, _buildingId;
        private readonly IssueRepository _issueRepository = new();
        public ObservableCollection<Issue> ReportedIssues { get; } = new();
        public bool HasReportedIssues { get => ReportedIssues.Count > 0; }

        public IssuesPage(long userId, long buildingId)
        {
            _userId = userId;
            _buildingId = buildingId;

            InitializeComponent();
            DataContext = this;

            UpdateIssues();
        }

        private void UpdateIssues()
        {
            ReportedIssues.Clear();

            var issues = _issueRepository.GetAllByBuildingAndResident(_buildingId, _userId);
            foreach (var issue in issues)
                ReportedIssues.Add(issue);

            OnPropertyChanged(nameof(HasReportedIssues));
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e)
            => Pop();

        private void ReportIssueButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
