using CommunityHub.Application.Services.Dtos.Issues;
using CommunityHub.Ui.Helpers.Alerts;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident.Buildings.Issues;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.Issues
{
    public partial class IssueDetailsPage : MainNavigationConsumerPage, IAlertServiceConsumer
    {
        public IssueDetailsPage(IssueDto issue)
        {
            InitializeComponent();
            DataContext = new IssueDetailsViewModel(issue, this);
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e)
            => Pop();
    }
}
