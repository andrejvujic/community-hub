using System.Windows.Input;
using CommunityHub.Ui.Helpers.Commands;

namespace CommunityHub.Ui.ViewModels.Citizen.LocalFacilities
{
    public class ReservationStatisticsViewModel
    {
        public event EventHandler? CloseRequested;

        public ICommand CloseCommand { get; }

        public ReservationStatisticsViewModel()
        {
            CloseCommand = new RelayCommand<object>(_ => CloseRequested?.Invoke(this, EventArgs.Empty));
        }
    }
}
