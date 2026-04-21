using CommunityHub.Application.Domain.NoticeBoard;
using CommunityHub.Ui.Helpers.Navigation;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.NoticeBoard
{
    public partial class CreateAdPage : MainNavigatorConsumerPage, INotifyPropertyChanged
    {
        private readonly long _userId, _buildingId;
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

        public AdCategory? SelectedCategory { get; set; }

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

        public DateTime? SelectedStartDate { get; set; } = DateTime.Now;
        public DateTime? SelectedEndDate { get; set; } = DateTime.Now;

        public string SelectedDateRangeText
        {
            get
            {
                if (SelectedStartDate is null || SelectedEndDate is null) return "Izaberite...";
                var culture = new CultureInfo("sr-Latn-RS");

                var formattedStartDate = SelectedStartDate.Value.ToString("d. MMMM", culture);
                var formattedEndDate = SelectedStartDate.Value.ToString("d. MMMM", culture);

                return $"{formattedStartDate} — {formattedEndDate}";
            }
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e) => Pop();

        public event PropertyChangedEventHandler? PropertyChanged;

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

        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
