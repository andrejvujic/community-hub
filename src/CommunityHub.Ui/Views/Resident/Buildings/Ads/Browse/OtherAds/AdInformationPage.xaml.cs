using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads;
using CommunityHub.Ui.Helpers.Navigation;

namespace CommunityHub.Ui.Views.Resident.Buildings.Ads.OtherAds
{
    public partial class AdInformationPage : MainNavigatorConsumerPage
    {
        public Ad Ad { get; }
        public AdInformationPage(Ad ad)
        {
            InitializeComponent();
            DataContext = this;

            Ad = ad;
        }
    }
}
