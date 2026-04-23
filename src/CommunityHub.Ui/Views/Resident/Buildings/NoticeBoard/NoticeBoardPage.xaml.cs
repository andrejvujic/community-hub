using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain.NoticeBoard;
using CommunityHub.Ui.Helpers.Navigation;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.NoticeBoard
{
    public partial class NoticeBoardPage : MainNavigatorConsumerPage
    {
        private readonly long _userId;
        private readonly long _buildingId;
        private readonly AdRepository _repository = new();
        public ObservableCollection<Ad> Ads { get; } = new();

        public NoticeBoardPage(long userId, long buildingId)
        {
            InitializeComponent();
            DataContext = this;

            _userId = userId;
            _buildingId = buildingId;

            UpdateAds();
        }

        private void UpdateAds()
        {
            Ads.Clear();
            List<Ad> ads = _repository.GetForBuilding(_buildingId);
            foreach (var ad in ads)
                Ads.Add(ad);
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e)
            => Pop();

        private void AdItem_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is not FrameworkElement element) return;

            if (element.DataContext is not Ad ad) return;

            if (ad.Author.Id == _userId)
            {
                Push(new PersonalAdDetailsPage(_userId, ad));
                return;
            }

            Push(new AdDetailsPage(_userId, ad));
        }
    }
}
