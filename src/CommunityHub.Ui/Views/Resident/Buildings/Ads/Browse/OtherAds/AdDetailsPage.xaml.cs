using CommunityHub.Ui.Helpers.Navigation;
using System.Windows.Input;
using CommunityHub.Application.Domain.Ads;
using CommunityHub.Application.Services.Dtos.Ads;

namespace CommunityHub.Ui.Views.Resident.Buildings.Ads.OtherAds
{
    public enum AdDetailsPageTab
    {
        Information,
        TimeSlots
    }

    public partial class AdDetailsPage : MainNavigationConsumerPage
    {
        private readonly long _userId;

        public AdDto Ad { get; }

        public AdDetailsPage(
            long userId,
            AdDto ad,
            AdDetailsPageTab initialTab = AdDetailsPageTab.Information
        )
        {
            InitializeComponent();
            DataContext = this;

            _userId = userId;
            Ad = ad;


            OpenInitialTab(initialTab);
        }

        private void OpenInitialTab(AdDetailsPageTab initialTab)
        {
            switch (initialTab)
            {
                case AdDetailsPageTab.Information:
                    OpenAdInformationPage();
                    break;
                case AdDetailsPageTab.TimeSlots:
                    OpenAdTimeSlotsPage();
                    break;
            }
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e)
            => Pop();

        private void BottomBar_InformationItemClick(object sender, MouseButtonEventArgs e)
            => OpenAdInformationPage();

        private void BottomBar_TimeSlotsItemClick(object sender, MouseButtonEventArgs e)
            => OpenAdTimeSlotsPage();

        private void OpenAdInformationPage() => LocalFrame.Navigate(new AdInformationPage(Ad));
        private void OpenAdTimeSlotsPage() => LocalFrame.Navigate(new AdTimeSlotsPage(_userId, Ad));
    }
}
