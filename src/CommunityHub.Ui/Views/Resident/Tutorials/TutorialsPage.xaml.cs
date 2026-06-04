using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident.Tutorials;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Tutorials
{
    public partial class TutorialsPage : MainNavigationConsumerPage
    {
        public TutorialsPage()
        {
            InitializeComponent();
            DataContext = new TutorialsViewModel(this);
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e)
            => Pop();
    }
}
