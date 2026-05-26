using CommunityHub.Application.Domain.ResidentAssemblies;
using CommunityHub.Application.Services.Dtos.ResidentAssemblies;
using CommunityHub.Application.Services.Dtos.Users;
using CommunityHub.Application.Services.ResidentAssemblies;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.Views.Resident.Buildings.Assemblies.Details;
using System.Collections.ObjectModel;

namespace CommunityHub.Ui.ViewModels.Resident.Buildings.Assemblies
{
    public class AssemblyTopicsViewModel : BaseViewModel
    {
        private readonly AssemblyTopicService _topicService = new();
        private readonly long _residentId;
        private readonly long _assemblyId;

        public ObservableCollection<AssemblyTopicAuthorGroup> TopicGroups { get; } = new();

        public bool HasTopics => TopicGroups.Count > 0;

        public RelayCommand OpenSuggestedTopicsCommand { get; }

        public AssemblyTopicsViewModel(
            long residentId,
            long assemblyId,
            IMainNavigationConsumer navigationConsumer
        ) : base(navigationConsumer)
        {
            _residentId = residentId;
            _assemblyId = assemblyId;
            OpenSuggestedTopicsCommand = new(OpenSuggestedTopicsPage);
        }

        public void UpdateTopics()
        {
            var acceptedTopics = _topicService.GetAllByAssembly(
                _assemblyId,
                AssemblyTopicStatus.Accepted
            );

            TopicGroups.Clear();

            foreach (var group in acceptedTopics.GroupBy(topic => topic.AuthorId))
            {
                var author = group.First().Author;
                if (author is null) continue;

                TopicGroups.Add(new AssemblyTopicAuthorGroup(author, group.ToList()));
            }

            OnPropertyChanged(nameof(HasTopics));
        }

        private void OpenSuggestedTopicsPage()
            => Push(new SuggestedAssemblyTopicsPage(_residentId, _assemblyId));
    }

    public class AssemblyTopicAuthorGroup
    {
        public UserDto Author { get; }

        public List<AssemblyTopicDto> Topics { get; }

        public bool IsEnforcedByManager => Topics.First().IsEnforcedByManager;

        public AssemblyTopicAuthorGroup(UserDto author, List<AssemblyTopicDto> topics)
        {
            Author = author;
            Topics = topics;
        }
    }
}
