using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityHub.Application.Services.Neighborhoods.CityFacilities;
using CommunityHub.Ui.Helpers.Commands;

namespace CommunityHub.Ui.ViewModels.Citizen.LocalFacilities
{
    public class FacilityVisitsViewModel
    {
        private readonly FacilityReservationService _facilityReservationService;

        public string FacilityName { get; set; }
        public ObservableCollection<DateTime> VisitDates { get; set; }
        public DateTime? SelectedVisitDate { get; set; }

        public event EventHandler? CloseRequested;

        public ICommand CloseCommand { get; }

        public FacilityVisitsViewModel(long facilityId, string facilityName, FacilityReservationService facilityReservationService)
        {
            _facilityReservationService = facilityReservationService;
            FacilityName = facilityName;
            VisitDates = new ObservableCollection<DateTime>();

            CloseCommand = new RelayCommand<object>(_ => CloseRequested?.Invoke(this, EventArgs.Empty));

            LoadVisitDates(facilityId);

            SelectedVisitDate = VisitDates.FirstOrDefault();
        }

        private void LoadVisitDates(long facilityId)
        {
            var reservations = _facilityReservationService.GetByFacilityId(facilityId);
            var sortedDates = reservations.Select(r => r.StartDatetime.Date).Distinct().OrderByDescending(d => d).ToList();

            foreach (var date in sortedDates)
            {
                VisitDates.Add(date);
            }
        }
    }
}
