using CommunityHub.Application.Services.Dtos.Ads;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident.Buildings.Ads;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.Ads.Browse.OtherAds
{
    public partial class MatchingAdsPage : MainNavigationConsumerPage
    {
        public MatchingAdsPage(long userId, AdDto ad)
        {
            InitializeComponent();
            DataContext = new MatchingAdsViewModel(userId, ad, this);
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e)
            => Pop();
    }
}
