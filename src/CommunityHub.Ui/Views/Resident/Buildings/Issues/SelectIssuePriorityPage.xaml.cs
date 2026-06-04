using CommunityHub.Application.Domain.Issues;
using CommunityHub.Ui.Helpers.Navigation;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.Issues
{
    public partial class SelectIssuePriorityPage : MainNavigationConsumerPage
    {
        public IssuePriority? SelectedPriority { get; }

        public SelectIssuePriorityPage(IssuePriority? selectedPriority = null)
        {
            InitializeComponent();
            DataContext = this;
            SelectedPriority = selectedPriority;
        }

        private void PriorityItem_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is not FrameworkElement element) return;
            if (element.DataContext is not IssuePriority priority) return;
            if (priority == SelectedPriority) return;

            Pop(priority);
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e)
            => Pop();
    }
}
