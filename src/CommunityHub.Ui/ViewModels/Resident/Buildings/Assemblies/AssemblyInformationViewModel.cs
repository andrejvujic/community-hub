using CommunityHub.Application.Domain.ResidentAssemblies;
using CommunityHub.Application.Services.Dtos.ResidentAssemblies;
using CommunityHub.Application.Services.ResidentAssemblies;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Helpers.Navigation;

namespace CommunityHub.Ui.ViewModels.Resident.Buildings.Assemblies
{
    public class AssemblyInformationViewModel : BaseViewModel
    {
        private readonly AssemblyAttendanceService _assemblyAttendanceService = new();
        private readonly long _residentId;

        public AssemblyDto Assembly { get; private set; }

        public int AttendancePercentage =>
            Assembly.BuildingApartmentCount > 0
                ? (int)(Assembly.Attendances.Count * 100.0 / Assembly.BuildingApartmentCount)
                : 0;

        public bool IsAttendanceConfirmed { get; private set; }

        public bool CanChangeAttendance =>
            Assembly.Status is not AssemblyStatus.Cancelled &&
            Assembly.StartsAt > DateTime.Now.AddHours(24);
            
        public string AttendanceActionText =>
            IsAttendanceConfirmed ? "Odjavi prisustvo" : "Prijavi prisustvo";

        public RelayCommand ToggleAttendanceCommand { get; }

        public AssemblyInformationViewModel(
            long residentId,
            AssemblyDto assembly,
            IMainNavigationConsumer navigationConsumer
        ) : base(navigationConsumer)
        {
            _residentId = residentId;
            Assembly = assembly;
            ToggleAttendanceCommand = new(ToggleAttendance, () => CanChangeAttendance);

            UpdateAttendanceState();
        }

        private void ToggleAttendance()
        {
            if (IsAttendanceConfirmed)
                Assembly = _assemblyAttendanceService.UnregisterAttendance(Assembly, _residentId);
            else
                Assembly = _assemblyAttendanceService.RegisterAttendance(Assembly, _residentId);

            UpdateAttendanceState();
        }

        private void UpdateAttendanceState()
        {
            IsAttendanceConfirmed = _assemblyAttendanceService.HasRegisteredAttendance(
                Assembly.Id,
                _residentId
            );

            OnPropertyChanged(nameof(Assembly));
            OnPropertyChanged(nameof(AttendancePercentage));
            OnPropertyChanged(nameof(IsAttendanceConfirmed));
            OnPropertyChanged(nameof(CanChangeAttendance));
            OnPropertyChanged(nameof(AttendanceActionText));
            ToggleAttendanceCommand.RaiseCanExecuteChanged();
        }
    }
}
