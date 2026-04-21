using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain.NoticeBoard;
using CommunityHub.Ui.Helpers.Navigation;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.NoticeBoard
{
    public partial class PersonalAdsPage : MainNavigatorConsumerPage, INotifyPropertyChanged
    {
        private readonly long _userId;
        private readonly AdRepository _repository = new();

        public ObservableCollection<Ad> Ads { get; } = new();

        public PersonalAdsPage(long userId)
        {
            InitializeComponent();
            DataContext = this;

            _userId = userId;

            UpdateAds();
        }

        private void UpdateAds()
        {
            Ads.Clear();
            List<Ad> ads = _repository.GetByAuthorId(_userId);
            foreach (var ad in ads)
                Ads.Add(ad);

            OnPropertyChanged(nameof(HasAds));
        }

        public bool HasAds { get => Ads.Count > 0; }


        private void BackButton_Click(object sender, MouseButtonEventArgs e)
            => Pop();

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
