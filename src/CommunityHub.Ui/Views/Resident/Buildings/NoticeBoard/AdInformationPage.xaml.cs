using CommunityHub.Application.Domain.NoticeBoard;
using CommunityHub.Ui.Helpers.Navigation;

namespace CommunityHub.Ui.Views.Resident.Buildings.NoticeBoard
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
