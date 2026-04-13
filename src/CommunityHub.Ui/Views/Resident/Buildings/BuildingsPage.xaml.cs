using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.Views.Resident.Buildings.Search;

namespace CommunityHub.Ui.Views.Resident.Buildings
{
    public partial class BuildingsPage : MainNavigatorConsumerPage
    {
        private readonly long _userId;
        public BuildingsPage(long userId)
        {
            InitializeComponent();
            Console.WriteLine(userId);
            _userId = userId;
        }

        private void CreateAppartmentAccessRequestButton_Click(object sender, System.Windows.RoutedEventArgs e)
            => Push(new SearchBuildingsPage(_userId));
    }
}
