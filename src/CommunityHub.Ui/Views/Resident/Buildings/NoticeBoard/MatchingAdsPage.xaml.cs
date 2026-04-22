using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain.NoticeBoard;
using CommunityHub.Ui.Helpers.Navigation;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Controls;

namespace CommunityHub.Ui.Views.Resident.Buildings.NoticeBoard
{
    public partial class MatchingAdsPage : MainNavigatorConsumerPage
    {
        private readonly AdRepository _adRepository = new();

        public Ad Ad { get; }
        public ObservableCollection<Ad> MatchingAds { get; set; } = new();

        public MatchingAdsPage(Ad ad)
        {
            InitializeComponent();
            DataContext = this;

            Ad = ad;

            UpdateAds();
        }

        private void UpdateAds()
        {
            MatchingAds.Clear();
            var ads = _adRepository.GetMatchingAds(Ad.Id);
            foreach (var ad in ads)
                MatchingAds.Add(ad);
        }
    }
}
