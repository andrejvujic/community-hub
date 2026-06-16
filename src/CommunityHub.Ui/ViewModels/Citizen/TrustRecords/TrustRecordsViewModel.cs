using CommunityHub.Application.Domain.Users;
using CommunityHub.Application.Services.Dtos.TrustRecords;
using CommunityHub.Application.Services.TrustRecords;
using CommunityHub.Application.Services.Users;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.ViewModels.Citizen.Help;
using CommunityHub.Ui.Views.Citizen.Help;
using System.ComponentModel;
using System.Windows.Input;

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
        public event EventHandler? ShowOtherMembersTrustRequested;
        public event EventHandler? ShowHelpRequested;

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

        public ICommand ShowOtherMembersTrustCommand { get; }
        public ICommand ShowHelpCommand { get; }

        public TrustRecordsViewModel(long userId)
        {
            _userId = userId;
            ShowOtherMembersTrustCommand = new RelayCommand<object>(_ => ShowOtherMembersTrustRequested?.Invoke(this, EventArgs.Empty));
            ShowHelpCommand = new RelayCommand<object>(_ => ShowHelp());

            LoadData();
        }

        private void ShowHelp()
        {
            var helpWindow = new QuickHelpWindow(
                "Brza pomoć - Evidencije poverenja",
                "Pregledajte svoju evidenciju poverenja u kvartu. Vidite informacije o svojem angažovanju u kvartu i status koji imate. " +
                "Možete videti i evidencije poverenja ostalih članova kvarta.",
                new List<KeyboardShortcut>
                {
                    new() { Keys = "F1-F7", Description = "Brza navigacija kroz kartice" },
                    new() { Keys = "Ctrl+T", Description = "Uključi/isključi prikaz objašnjenja" },
                    new() { Keys = "Ctrl+1", Description = "Evidencije poverenja ostalih članova kvarta" },
                }
            );
            helpWindow.ShowDialog();
        }

        private void LoadData()
        {
            TrustRecord = _trustRecordService.GetTrustRecord(_userId);
            var citizen = _citizenService.GetCitizenById(_userId);
            NeighborhoodId = citizen?.Membership?.Neighborhood.Id;
        }

        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
