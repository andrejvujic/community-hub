using CommunityHub.Ui.Helpers.Navigation;
using System.Windows.Input;
using CommunityHub.Application.Domain.NoticeBoard;

namespace CommunityHub.Ui.Views.Resident.Buildings.NoticeBoard
{
    public enum PersonalAdDetailsPageTab
    {
        Information,
        TimeSlots
    }

    public partial class PersonalAdDetailsPage : MainNavigatorConsumerPage
    {
        private readonly long _userId;

        public Ad Ad { get; }

        public PersonalAdDetailsPage(
            long userId,
            Ad ad,
            PersonalAdDetailsPageTab initialTab = PersonalAdDetailsPageTab.Information
        )
        {
            InitializeComponent();
            DataContext = this;

            _userId = userId;
            Ad = ad;

            OpenInitialTab(initialTab);
        }

        private void OpenInitialTab(PersonalAdDetailsPageTab initialTab)
        {
                switch (initialTab)
                {
                    case PersonalAdDetailsPageTab.Information:
                        OpenAdInformationPage();
                        break;
                    case PersonalAdDetailsPageTab.TimeSlots:
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

        private void OpenAdInformationPage() => LocalFrame.Navigate(new PersonalAdInformationPage(Ad));
        private void OpenAdTimeSlotsPage() => LocalFrame.Navigate(new PersonalAdTimeSlotsPage(_userId, Ad));
    }
}
