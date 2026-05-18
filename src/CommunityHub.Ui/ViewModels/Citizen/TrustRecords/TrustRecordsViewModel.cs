using CommunityHub.Application.Services.Dtos.TrustRecords;
using CommunityHub.Application.Services.TrustRecords;
using CommunityHub.Application.Services.Users;
using System.ComponentModel;

namespace CommunityHub.Ui.ViewModels.Citizen.TrustRecords
{
    public class TrustRecordsViewModel : INotifyPropertyChanged
    {
        private readonly long _userId;
        private readonly CitizenTrustRecordService _trustRecordService = new();
        private readonly CitizenService _citizenService = new();

        private CitizenTrustRecordDto? _trustRecord;
        private long? _neighborhoodId;

        public event PropertyChangedEventHandler? PropertyChanged;

        public CitizenTrustRecordDto? TrustRecord
        {
            get => _trustRecord;
            private set
            {
                if (_trustRecord == value) return;
                _trustRecord = value;
                OnPropertyChanged(nameof(TrustRecord));
            }
        }

        public long? NeighborhoodId
        {
            get => _neighborhoodId;
            private set
            {
                if (_neighborhoodId == value) return;
                _neighborhoodId = value;
                OnPropertyChanged(nameof(NeighborhoodId));
            }
        }

        public TrustRecordsViewModel(long userId)
        {
            _userId = userId;
            LoadData();
        }

        private void LoadData()
        {
            TrustRecord = _trustRecordService.GetTrustRecord(_userId);
            NeighborhoodId = _citizenService.GetNeighborhoodIdForCitizen(_userId);
        }

        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
