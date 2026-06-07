using CommunityHub.Application.Services.Dtos.TrustRecords;
using CommunityHub.Application.Services.TrustRecords;
using CommunityHub.Ui.Helpers.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace CommunityHub.Ui.ViewModels.Citizen.TrustRecords
{
    public class OtherMembersTrustViewModel : INotifyPropertyChanged
    {
        private readonly long _userId;
        private readonly long _neighborhoodId;
        private readonly CitizenTrustRecordService _trustRecordService = new();
        private CitizenTrustRecordDto? _selectedTrustRecord;

        private ObservableCollection<CitizenTrustRecordDto> _trustRecords = new();

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler? CloseRequested;

        public ICommand CloseCommand { get; }

        public ObservableCollection<CitizenTrustRecordDto> TrustRecords
        {
            get => _trustRecords;
            private set
            {
                if (_trustRecords == value) return;
                _trustRecords = value;
                OnPropertyChanged(nameof(TrustRecords));
            }
        }

        public CitizenTrustRecordDto? SelectedTrustRecord
        {
            get => _selectedTrustRecord;
            set
            {
                if (_selectedTrustRecord == value) return;
                _selectedTrustRecord = value;
                OnPropertyChanged(nameof(SelectedTrustRecord));
            }
        }

        public OtherMembersTrustViewModel(long userId, long neighborhoodId)
        {
            _userId = userId;
            _neighborhoodId = neighborhoodId;
            CloseCommand = new RelayCommand<object>(_ => CloseRequested?.Invoke(this, EventArgs.Empty));
            LoadData();
        }

        private void LoadData()
        {
            var records = _trustRecordService.GetNeighborhoodTrustRecords(_neighborhoodId, _userId);
            TrustRecords = new ObservableCollection<CitizenTrustRecordDto>(records);

            SelectedTrustRecord = TrustRecords.FirstOrDefault();
        }

        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
