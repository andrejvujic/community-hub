using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident.Buildings.Issues;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.Issues
{
    public partial class IssuesPage : MainNavigationConsumerPage
    {
        private readonly IssuesViewModel _viewModel;

        public IssuesPage(long userId, long buildingId)
        {
            InitializeComponent();
            _viewModel = new IssuesViewModel(userId, buildingId, this);
            DataContext = _viewModel;

            _viewModel.UpdateIssues();
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e)
            => Pop();

        private void ReportIssueButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
