using CommunityHub.Application.Services.Dtos.Ads;
using CommunityHub.Ui.Helpers.Navigation;

namespace CommunityHub.Ui.Views.Resident.Buildings.Ads.OtherAds
{
    public partial class AdInformationPage : MainNavigatorConsumerPage
    {
        public AdDto Ad { get; }
        public AdInformationPage(AdDto ad)
        {
            InitializeComponent();
            DataContext = this;

            Ad = ad;
        }
    }
}
