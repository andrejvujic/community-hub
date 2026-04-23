using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain.NoticeBoard;
using CommunityHub.Ui.Helpers.Navigation;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.NoticeBoard
{
    public partial class PersonalAdsPage : MainNavigatorConsumerPage, INotifyPropertyChanged
    {
        private readonly long _userId, _buildingId;
        private readonly AdRepository _repository = new();

        public ObservableCollection<Ad> Ads { get; } = new();

        public PersonalAdsPage(long userId, long buildingId)
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
            List<Ad> ads = _repository.GetAuthorAdsForBuilding(_userId, _buildingId);
            foreach (var ad in ads)
                Ads.Add(ad);

            OnPropertyChanged(nameof(HasAds));
        }

        public bool HasAds { get => Ads.Count > 0; }

        private void CreateAdButton_Click(object sender, RoutedEventArgs e)
        {
            Push(
                new CreateAdPage(
                    userId: _userId,
                    buildingId: _buildingId
                ),
                onPop: (object? value) =>
                {
                    if (value is Ad publishedAd)
                    {
                        UpdateAds();
                        Push(new MatchingAdsPage(publishedAd));
                        return;
                    }
                }
             );
         }

        private void BackButton_Click(object sender, MouseButtonEventArgs e)
            => Pop();

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
