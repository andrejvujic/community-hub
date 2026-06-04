using CommunityHub.Ui.Helpers.Alerts;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident.Buildings.Issues;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.Issues
{
    public partial class CreateIssuePage : MainNavigationConsumerPage, IAlertServiceConsumer
    {
        public CreateIssuePage(long userId, long buildingId)
        {
            InitializeComponent();
            DataContext = new CreateIssueViewModel(userId, buildingId, this);
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e)
            => Pop();
    }
}
