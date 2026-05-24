using CommunityHub.Application.Services.Dtos.Ads;
using CommunityHub.Ui.Helpers.Alerts;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident.Buildings.Ads;

namespace CommunityHub.Ui.Views.Resident.Buildings.Ads.Browse.MyAds
{
    public partial class MyAdInfoPage : MainNavigationConsumerPage, IAlertServiceConsumer
    {
        public MyAdInfoPage(AdDto ad)
        {
            InitializeComponent();
            DataContext = new MyAdInfoViewModel(ad, this);
        }
    }
}
