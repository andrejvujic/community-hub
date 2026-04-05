using CommunityHub.Ui.Helpers.Navigation;
using System.Windows.Controls;

namespace CommunityHub.Ui.Views.BuildingResident
{
    public partial class SearchBuildingsPage : MainNavigatorConsumerPage
    {
        private readonly long _userId;
        public SearchBuildingsPage(long userId)
        {
            InitializeComponent();
            _userId = userId;
        }

        private void BackButton_Click(object sender, System.Windows.Input.MouseButtonEventArgs e) => Pop();
    }
}
