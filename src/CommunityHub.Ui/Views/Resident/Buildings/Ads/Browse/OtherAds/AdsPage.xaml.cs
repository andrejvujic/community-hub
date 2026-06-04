using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident.Buildings.Ads;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.Ads.Browse.OtherAds
{
    public partial class AdsPage : MainNavigationConsumerPage
    {
        public AdsPage(long userId, long buildingId)
        {
            InitializeComponent();
            DataContext = new AdsViewModel(userId, buildingId, this);
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e)
            => Pop();
    }
}
