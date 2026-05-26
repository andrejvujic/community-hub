using CommunityHub.Application.Services.Dtos.ResidentAssemblies;
using CommunityHub.Application.Services.ResidentAssemblies;
using CommunityHub.Ui.Helpers.Navigation;
using System.Collections.ObjectModel;

namespace CommunityHub.Ui.ViewModels.Resident.Buildings.Assemblies
{
    public class SuggestedAssemblyTopicsViewModel : BaseViewModel
    {
        private readonly AssemblyTopicService _topicService = new();
        private readonly long _residentId;
        private readonly long _assemblyId;

        public ObservableCollection<AssemblyTopicDto> Topics { get; } = new();

        public bool HasTopics => Topics.Count > 0;

        public SuggestedAssemblyTopicsViewModel(
            long residentId,
            long assemblyId,
            IMainNavigationConsumer navigationConsumer
        ) : base(navigationConsumer)
        {
            _residentId = residentId;
            _assemblyId = assemblyId;
        }

        public void UpdateTopics()
        {
            var topics = _topicService.GetAllByAssemblyAndAuthor(_assemblyId, _residentId);

            Topics.Clear();

            foreach (var topic in topics)
                Topics.Add(topic);

            OnPropertyChanged(nameof(HasTopics));
        }
    }
}
