using CommunityHub.Application;
using CommunityHub.Application.Modules.Buildings.API.Public.Ads;
using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads;
using CommunityHub.Application.Modules.Buildings.Database.Repositories.Ads;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.Views.Resident.Buildings.Ads.Browse.OtherAds;
using CommunityHub.Ui.Views.Resident.Buildings.Ads.Create;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.Ads.Browse.MyAds
{
    public partial class MyAdsPage : MainNavigatorConsumerPage, INotifyPropertyChanged
    {
        private readonly long _userId, _buildingId;
        private readonly IAdService _adService = Injector.CreateInstance<IAdService>();

        public ObservableCollection<Ad> Ads { get; } = new();
        public bool HasAds => Ads.Count > 0; 

        public MyAdsPage(long userId, long buildingId)
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
            List<Ad> ads = _adService.GetByAuthorAndBuilding(_userId, _buildingId);
            foreach (var ad in ads)
                Ads.Add(ad);

            OnPropertyChanged(nameof(HasAds));
        }

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
                        Push(new MatchingAdsPage(_userId, publishedAd));
                        return;
                    }
                }
             );
         }

        private void AdItem_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is not FrameworkElement element) return;

            if (element.DataContext is not Ad ad) return;

            Push(
                new MyAdDetailsPage(_userId, ad),
                onPop: _ => UpdateAds()
            );
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e)
            => Pop();

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
