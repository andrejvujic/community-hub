using CommunityHub.Application.Domain.NoticeBoard;
using CommunityHub.Ui.Helpers.Navigation;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.NoticeBoard
{
    public partial class CreateAdPage : MainNavigatorConsumerPage, INotifyPropertyChanged
    {
        private readonly long _userId, _buildingId;
        private readonly CultureInfo _culture = new CultureInfo("sr-Latn-RS");

        public CreateAdPage(long userId, long buildingId)
        {
            InitializeComponent();
            DataContext = this;

            _userId = userId;
            _buildingId = buildingId;
        }

        private AdType? _selectedType;
        public AdType? SelectedType
        {
            get => _selectedType;
            set {
                if (_selectedType == value) return;
                _selectedType = value;
                OnPropertyChanged(nameof(SelectedType));
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
                _selectedStartDate = value;
                OnPropertyChanged(nameof(SelectedStartDate));
                OnPropertyChanged(nameof(SelectedStartDateText));
                OnPropertyChanged(nameof(SelectedDateRangeText));
                OnPropertyChanged(nameof(MinEndDate));
                OnPropertyChanged(nameof(IsDateRangeSelected));
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
                _selectedEndDate = value;
                OnPropertyChanged(nameof(SelectedEndDate));
                OnPropertyChanged(nameof(SelectedEndDateText));
                OnPropertyChanged(nameof(SelectedDateRangeText));
                OnPropertyChanged(nameof(MaxStartDate));
                OnPropertyChanged(nameof(IsDateRangeSelected));
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

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OpenStartDatePickerButton_Click(object sender, MouseButtonEventArgs e)
            => StartDatePicker.Visibility = Visibility.Visible;

        private void CloseStartDatePickerButton_Click(object sender, RoutedEventArgs e)
            => StartDatePicker.Visibility = Visibility.Collapsed;

        private void OpenEndDatePickerButton_Click(object sender, MouseButtonEventArgs e)
            => EndDatePicker.Visibility = Visibility.Visible;
        private void CloseEndDatePickerButton_Click(object sender, RoutedEventArgs e)
            => EndDatePicker.Visibility = Visibility.Collapsed;


        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
