using CommunityHub.Application;
using CommunityHub.Application.Modules.Buildings.API.Public.Ads;
using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads;
using CommunityHub.Application.Modules.Shared.API.Public.Users;
using CommunityHub.Ui.Helpers.Alerts;
using CommunityHub.Ui.Helpers.Navigation;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.Ads.Create
{
    public enum PublishmentValidationResult
    {
        Valid,
        MatchingAdExists,
    }

    public partial class CreateAdPage : MainNavigatorConsumerPage, IAlertConsumer, INotifyPropertyChanged
    {
        private readonly long _userId, _buildingId;

        private readonly CultureInfo _culture = new("sr-Latn-RS");
        private readonly DateTime _today = DateTime.Today;

        private readonly IUserService _userService = Injector.CreateInstance<IUserService>();
        private readonly IAdService _adService = Injector.CreateInstance<IAdService>();
        private readonly IAdMatchSubscriptionService _adMatchSubscriptionService = Injector.CreateInstance<IAdMatchSubscriptionService>();
        
        public CreateAdPage(long userId, long buildingId)
        {
            InitializeComponent();
            DataContext = this;

            _userId = userId;
            _buildingId = buildingId;
        }

        private Ad? CreateAd()
        {
            var author = _userService.Get(_userId);

            if (author is null) return null;

            return new Ad(
                author: author,
                buildingId: _buildingId,
                type: SelectedType!.Value,
                category: SelectedCategory!.Value,
                status: AdStatus.Active,
                description: Description,
                startDate: SelectedStartDate!.Value,
                endDate: SelectedEndDate!.Value
            );
        }

        private AdType? _selectedType;
        public AdType? SelectedType
        {
            get => _selectedType;
            set {
                if (_selectedType == value) return;
                _selectedType = value;
                OnPropertyChanged(nameof(SelectedType));
                OnPropertyChanged(nameof(IsFormValid));
            }
        }

        private AdCategory? _selectedCategory;
        public AdCategory? SelectedCategory {
            get => _selectedCategory;
            set
            {
                if (_selectedCategory == value) return; 
                _selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
                OnPropertyChanged(nameof(IsFormValid));
            }
        }

        private string _description = string.Empty;
        public string Description
        {
            get => _description;
            set {
                if (_description == value) return;
                _description = value;
                OnPropertyChanged(nameof(Description));
                OnPropertyChanged(nameof(IsFormValid));
            }
        }

        // The start date can't be in the past.
        public DateTime MinStartDate => _today; 

        // The start date can't be after the end date.
        public DateTime? MaxStartDate => SelectedEndDate; 

        // The end date can't be before the start date
        // (or in the past if the start date hasn't been
        // selected yet).
        public DateTime MinEndDate => SelectedStartDate ?? _today; 


        private DateTime? _selectedStartDate;
        public DateTime? SelectedStartDate
        {
            get => _selectedStartDate;
            set
            {
                if (_selectedStartDate == value) return;
                _selectedStartDate = value is null ? null : value.Value.Date;

                OnPropertyChanged(nameof(SelectedStartDate));
                OnPropertyChanged(nameof(SelectedStartDateText));
                OnPropertyChanged(nameof(SelectedDateRangeText));

                OnPropertyChanged(nameof(MinEndDate));

                OnPropertyChanged(nameof(IsSelectedDateRangeValid));
                OnPropertyChanged(nameof(IsFormValid));
            }
        }

        public string SelectedStartDateText
            => SelectedStartDate is null ?
                "Izaberite..." :
                SelectedStartDate.Value.ToString("d. MMMM", _culture);


        private DateTime? _selectedEndDate;
        public DateTime? SelectedEndDate
        {
            get => _selectedEndDate;
            set
            {
                if (_selectedEndDate == value) return;
                _selectedEndDate = value is null ? null : value.Value.Date;

                OnPropertyChanged(nameof(SelectedEndDate));
                OnPropertyChanged(nameof(SelectedEndDateText));
                OnPropertyChanged(nameof(SelectedDateRangeText));

                OnPropertyChanged(nameof(MaxStartDate));

                OnPropertyChanged(nameof(IsSelectedDateRangeValid));
                OnPropertyChanged(nameof(IsFormValid));
            }
        }

        public string SelectedEndDateText
            => SelectedEndDate is null ?
                "Izaberite..." :
                SelectedEndDate.Value.ToString("d. MMMM", _culture);

        public string SelectedDateRangeText
        {
            get
            {
                if (SelectedStartDate is null || SelectedEndDate is null) return "Izaberite...";
                return $"{SelectedStartDateText} — {SelectedEndDateText}";
            }
        }

        public bool IsSelectedDateRangeValid {
            get
            {
                if (SelectedStartDate is null || SelectedEndDate is null) return false;
                if (SelectedStartDate.Value < _today) return false;
                if (SelectedEndDate.Value < SelectedStartDate.Value) return false;

                return true;
            }
        }

        public bool IsFormValid
        { 
            get
            {
                if (!IsSelectedDateRangeValid) return false;

                if (string.IsNullOrWhiteSpace(Description)) return false;

                if (SelectedType is null || SelectedCategory is null) return false;

                return true;
            }       
        }

        private PublishmentValidationResult ValidateAdPublishment(Ad ad)
        {
            // User can't have more than one active ad in the
            // same building with the same type and category
            // whose date ranges overlap.
            if (_adService.HasMatchingAds(_userId, ad))
                return PublishmentValidationResult.MatchingAdExists;

            return PublishmentValidationResult.Valid;
        }

        private void PublishAdButton_Click(object sender, RoutedEventArgs e)
        {
            Ad? ad = CreateAd();
            if (ad == null) return;

            PublishmentValidationResult result = ValidateAdPublishment(ad);

            if (result == PublishmentValidationResult.Valid)
            {
                var publishedAd = _adService.Publish(ad);
                _adMatchSubscriptionService.NotifyAuthorsOfSubscribedMatchingAds(publishedAd);
                ShowSuccessAlert(publishedAd);   
                return;
            }

            ShowPublishmentValidationResult(result);
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e) => Pop();

        private void SelectTypeButton_Click(object sender, MouseButtonEventArgs e)
        {
            Push(
                new SelectAdTypePage(selectedType: SelectedType),
                onPop: (object? value) => {
                    if (value is not AdType type) return;
                    SelectedType = type;
                }
            );
        }

        private void SelectCategoryButton_Click(object sender, MouseButtonEventArgs e)
        {
            Push(
                new SelectAdCategoryPage(selectedCategory: SelectedCategory),
                onPop: (object? value) => {
                    if (value is not AdCategory category) return;
                    SelectedCategory = category;
                }
            );
        }

        private void ShowPublishmentValidationResult(PublishmentValidationResult result)
        {
            switch (result)
            {
                case PublishmentValidationResult.MatchingAdExists:
                    if (this is IAlertConsumer consumer)
                        consumer.ShowAlert(
                            Alert.Create(
                                "Greška",
                                "Objava oglasa nije bila moguća. Već imate aktivan oglas u ovoj zgradi sa istim tipom, kategorijom i preklapajućim datumima. Korigujte opseg datuma i pokušajte ponovo.",
                                AlertType.Error
                            )
                            .AddOption("U redu")
                        );  
                    break;
            }
        }

        private void ShowSuccessAlert(Ad publishedAd)
        {
            if (this is IAlertConsumer consumer)
                consumer.ShowAlert(
                    Alert.Create(
                        "Oglas objavljen",
                        "Vaš oglas je uspješno objavljen i biće prikazan ostalim stanarima na oglasnoj tabli zgrade.",
                        AlertType.Info
                    )
                    .AddOption(
                        "Nastavi",
                        () => Pop(publishedAd)
                    )
                );
        }

        private void ToggleOverlayVisibility(FrameworkElement overlay)
            =>
                overlay.Visibility = overlay.Visibility switch
                {
                    Visibility.Visible => Visibility.Collapsed,
                    Visibility.Collapsed => Visibility.Visible,
                    _ => Visibility.Visible
                };

        private void ToggleSelectDateRangeMenuButton_Click(object sender, EventArgs e)
            => ToggleOverlayVisibility(SelectDateRangeOverlay);

        private void ToggleStartDatePickerButton_Click(object sender, EventArgs e)
            => ToggleOverlayVisibility(StartDatePicker);

        private void ToggleEndDatePickerButton_Click(object sender, EventArgs e)
            => ToggleOverlayVisibility(EndDatePicker);


        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
