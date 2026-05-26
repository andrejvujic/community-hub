using CommunityHub.Application.Domain.ResidentAssemblies;
using CommunityHub.Application.Services.Dtos.ResidentAssemblies;
using CommunityHub.Application.Services.ResidentAssemblies;
using CommunityHub.Ui.Helpers.Navigation;
using System.Collections.ObjectModel;

namespace CommunityHub.Ui.ViewModels.Resident.Buildings.Assemblies
{
    public class AssembliesViewModel : BaseViewModel
    {
        private readonly AssemblyService _assemblyService = new();
        private readonly long _buildingId;

        public ObservableCollection<AssemblyDto> UpcomingAssemblies { get; } = new();
        public ObservableCollection<AssemblyDto> PreviousAssemblies { get; } = new();

        public bool HasAssemblies => HasUpcomingAssemblies || HasPreviousAssemblies;
        public bool HasUpcomingAssemblies => UpcomingAssemblies.Count > 0;
        public bool HasPreviousAssemblies => PreviousAssemblies.Count > 0;

        public AssembliesViewModel(
            long buildingId,
            IMainNavigationConsumer navigationConsumer
        ) : base(navigationConsumer)
        {
            _buildingId = buildingId;
        }

        public void UpdateAssemblies()
        {
            var now = DateTime.Now;
            var assemblies = _assemblyService.GetAllByBuilding(_buildingId);

            UpcomingAssemblies.Clear();
            PreviousAssemblies.Clear();

            foreach (var assembly in assemblies
                .Where(assembly => assembly.StartsAt >= now && assembly.Status is not AssemblyStatus.Cancelled)
                .OrderBy(assembly => assembly.StartsAt))
                UpcomingAssemblies.Add(assembly);

            foreach (var assembly in assemblies
                .Where(assembly => assembly.StartsAt < now || assembly.Status is AssemblyStatus.Cancelled)
                .OrderByDescending(assembly => assembly.StartsAt))
                PreviousAssemblies.Add(assembly);

            OnPropertyChanged(nameof(HasAssemblies));
            OnPropertyChanged(nameof(HasUpcomingAssemblies));
            OnPropertyChanged(nameof(HasPreviousAssemblies));
        }
    }
}
