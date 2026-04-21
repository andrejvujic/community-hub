using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain.NoticeBoard;
using CommunityHub.Ui.Helpers.Navigation;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.NoticeBoard
{
    public partial class NoticeBoardPage : MainNavigatorConsumerPage
    {
        private readonly long _buildingId;
        private readonly AdRepository _repository = new();
        public ObservableCollection<Ad> Ads { get; } = new();

        public NoticeBoardPage(long buildingId)
        {
            InitializeComponent();
            DataContext = this;

            _buildingId = buildingId;

            UpdateAds();
        }

        private void UpdateAds()
        {
            Ads.Clear();
            List<Ad> ads = _repository.GetByBuildingId(_buildingId);
            foreach (var ad in ads)
                Ads.Add(ad);
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e)
            => Pop();
    }
}
