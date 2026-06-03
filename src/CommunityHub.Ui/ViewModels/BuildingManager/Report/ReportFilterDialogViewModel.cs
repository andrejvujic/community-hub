using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.ViewModels.BuildingManager.Navigation;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.ViewModels.BuildingManager.Report
{
    public class ReportFilterDialogViewModel : INotifyPropertyChanged
    {
        private BuildingReportOptionViewModel? _selectedBuildingOption;
        private DateTime? _fromDate;
        private DateTime? _toDate;
        private string _validationMessage = string.Empty;

        public ObservableCollection<BuildingReportOptionViewModel> BuildingOptions { get; }

        public BuildingReportOptionViewModel? SelectedBuildingOption
        {
            get => _selectedBuildingOption;
            set
            {
                _selectedBuildingOption = value;
                OnPropertyChanged();
            }
        }
        public DateTime? FromDate
        {
            get => _fromDate;
            set
            {
                _fromDate = value;
                OnPropertyChanged();
            }
        }
        public DateTime? ToDate
        {
            get => _toDate;
            set
            {
                _toDate = value;
                OnPropertyChanged();
            }
        }
        public string ValidationMessage
        {
            get => _validationMessage;
            set
            {
                _validationMessage = value;
                OnPropertyChanged();
            }
        }

        public ICommand ConfirmCommand { get; }
        public ICommand CancelCommand { get; }

        public ReportFilterDialogViewModel(IEnumerable<BuildingReportOptionViewModel> buildingOptions)
        {
            BuildingOptions = new ObservableCollection<BuildingReportOptionViewModel>(buildingOptions);

            SelectedBuildingOption = BuildingOptions.FirstOrDefault();
            FromDate = DateTime.Today.AddMonths(-1);
            ToDate = DateTime.Today;

            ConfirmCommand = new RelayCommand<Window?>(_ => Confirm(_));
            CancelCommand = new RelayCommand<Window?>(_ => Cancel(_));
        }

        private void Confirm(Window? window)
        {
            if (FromDate > ToDate)
            {
                ValidationMessage = "Početni datum ne može biti veći od krajnjeg.";
                return;
            }

            ValidationMessage = string.Empty;

            if (window != null)
                window.DialogResult = true;
        }
        private void Cancel(Window? window)
        {
            if(window != null) 
                window.DialogResult = false;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}