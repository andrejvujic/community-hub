using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.NoticeBoard;
using CommunityHub.Ui.Helpers.Alerts;
using CommunityHub.Ui.Helpers.Navigation;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.NoticeBoard
{
    public partial class CreateAdPage : MainNavigatorConsumerPage, IAlertConsumer, INotifyPropertyChanged
    {
        private readonly long _userId, _buildingId;
        private readonly CultureInfo _culture = new("sr-Latn-RS");

        private readonly UserRepository _userRepository = new();
        private readonly AdRepository _adRepository = new();

        public CreateAdPage(long userId, long buildingId)
        {
            InitializeComponent();
            DataContext = this;

            _userId = userId;
            _buildingId = buildingId;
        }

        private Ad? CreateAd()
        {
            var author = _userRepository.GetById(_userId);
            if (author == null) return null;

            return new Ad(
                    author,
                    _buildingId,
                    SelectedType!.Value,
                    SelectedCategory!.Value,
                    AdStatus.Active,
                    Description,
                    SelectedStartDate!.Value,
                    SelectedEndDate!.Value
                );
        }

        private void ShowErrorAlert()
        {
            if (this is IAlertConsumer consumer)
                consumer.ShowAlert(
                    Alert.Create(
                        "Greška",
                        "Došlo je do greške prilikom objavljivanja oglasa. Provjerite da li ste ispravno unijeli sve informacije i pokušajte ponovo.",
                        AlertType.Error
                    ).AddOption("U redu")
                );
        }

        private void ShowSuccessAlert(long publishedAdId)
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
                        () => Pop(publishedAdId)
                    )
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
        public DateTime MinStartDate => DateTime.Now; 

        // The start date can't be after the end date.
        public DateTime? MaxStartDate => SelectedEndDate; 

        // The end date can't be before the start date
        // (or in the past if the start date hasn't been
        // selected yet).
        public DateTime MinEndDate => SelectedStartDate ?? DateTime.Now; 


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
                OnPropertyChanged(nameof(IsDateRangeSelected));
                OnPropertyChanged(nameof(IsFormValid));
            }
        }
        public string SelectedStartDateText {
            get
            {
                if (SelectedStartDate == null) return "Izaberite...";
                return SelectedStartDate.Value.ToString("d. MMMM", _culture);
            }
        }

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
                OnPropertyChanged(nameof(IsDateRangeSelected));
                OnPropertyChanged(nameof(IsFormValid));
            }
        }

        public string SelectedEndDateText
        {
            get
            {
                if (SelectedEndDate == null) return "Izaberite...";
                return SelectedEndDate.Value.ToString("d. MMMM", _culture);
            }
        }

        public string SelectedDateRangeText
        {
            get
            {
                if (SelectedStartDate is null || SelectedEndDate is null) return "Izaberite...";
                return $"{SelectedStartDateText} — {SelectedEndDateText}";
            }
        }

        public bool IsDateRangeSelected => SelectedStartDate is not null && SelectedEndDate is not null;

        public bool IsSelectedDateRangeValid()
        {
            if (!IsDateRangeSelected) return false;
            if (SelectedStartDate!.Value < DateTime.Now) return false;
            if (SelectedEndDate!.Value < SelectedStartDate.Value) return false;
            return true;
        }

        public bool IsFormValid
        { 
            get
            {
                if (!IsSelectedDateRangeValid()) return false;

                if (string.IsNullOrWhiteSpace(Description)) return false;

                if (SelectedType == null || SelectedCategory == null) return false;

                return true;
            }       
        }

        private void PublishAdButton_Click(object sender, RoutedEventArgs e)
        {
            var ad = CreateAd();

            if (ad == null)
            {
                ShowErrorAlert();
                return;
            }

            var publishedAd = _adRepository.Create(ad);
            ShowSuccessAlert(publishedAd.Id);
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

        private void OpenSelectDateRangeMenuButton_Click(object sender, MouseButtonEventArgs e)
            => SelectDateRangeOverlay.Visibility = Visibility.Visible;
        private void CloseSelectDateRangeMenuButton_Click(object sender, RoutedEventArgs e)
            => SelectDateRangeOverlay.Visibility = Visibility.Collapsed;

        private void OpenStartDatePickerButton_Click(object sender, MouseButtonEventArgs e)
            => StartDatePicker.Visibility = Visibility.Visible;

        private void CloseStartDatePickerButton_Click(object sender, RoutedEventArgs e)
            => StartDatePicker.Visibility = Visibility.Collapsed;

        private void OpenEndDatePickerButton_Click(object sender, MouseButtonEventArgs e)
            => EndDatePicker.Visibility = Visibility.Visible;
        private void CloseEndDatePickerButton_Click(object sender, RoutedEventArgs e)
            => EndDatePicker.Visibility = Visibility.Collapsed;


        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
