using CommunityHub.Ui.Helpers.Navigation;
using System.Windows.Input;
using CommunityHub.Application.Domain.Ads;
using CommunityHub.Application.Services.Dtos.Ads;

namespace CommunityHub.Ui.Views.Resident.Buildings.Ads.Browse.MyAds
{
    public enum MyAdDetailsPageTab
    {
        Information,
        TimeSlots
    }

    public partial class MyAdDetailsPage : MainNavigationConsumerPage
    {
        private readonly long _userId;

        public AdDto Ad { get; }

        public MyAdDetailsPage(
            long userId,
            AdDto ad,
            MyAdDetailsPageTab initialTab = MyAdDetailsPageTab.Information
        )
        {
            InitializeComponent();
            DataContext = this;

            _userId = userId;
            Ad = ad;

            OpenInitialTab(initialTab);
        }

        private void OpenInitialTab(MyAdDetailsPageTab initialTab)
        {
            switch (initialTab)
            {
                case MyAdDetailsPageTab.Information:
                    OpenAdInformationPage();
                    break;
                case MyAdDetailsPageTab.TimeSlots:
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

        private void OpenAdInformationPage() => LocalFrame.Navigate(new MyAdInfoPage(Ad));
        private void OpenAdTimeSlotsPage() => LocalFrame.Navigate(new MyAdTimeSlotsPage(_userId, Ad));
    }
}