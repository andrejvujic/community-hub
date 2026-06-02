using CommunityHub.Application.Services.Dtos.ResidentAssemblies;
using CommunityHub.Application.Services.ResidentAssemblies;
using CommunityHub.Ui.Helpers.Alerts;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Helpers.Navigation;
using System.Collections.ObjectModel;

namespace CommunityHub.Ui.ViewModels.Resident.Buildings.Assemblies
{
    public class SuggestedAssemblyTopicsViewModel : BaseViewModel
    {
        private readonly AssemblyService _assemblyService = new();
        private readonly AssemblyTopicService _topicService = new();
        private readonly long _residentId;
        private readonly long _assemblyId;

        public ObservableCollection<AssemblyTopicDto> Topics { get; } = new();

        public bool HasTopics => Topics.Count > 0;

        public RelayCommand SuggestTopicCommand { get; }
        public RelayCommand<AssemblyTopicDto> RemoveTopicCommand { get; }

        public SuggestedAssemblyTopicsViewModel(
            long residentId,
            long assemblyId,
            IMainNavigationConsumer navigationConsumer
        ) : base(navigationConsumer)
        {
            _residentId = residentId;
            _assemblyId = assemblyId;
            SuggestTopicCommand = new(SuggestTopic, () => IsFormValid);
            RemoveTopicCommand = new(RemoveTopic);
        }

        public void UpdateTopics()
        {
            var topics = _topicService.GetAllByAssemblyAndAuthor(_assemblyId, _residentId);

            Topics.Clear();

            foreach (var topic in topics)
                Topics.Add(topic);

            OnPropertyChanged(nameof(HasTopics));
        }

        private void SuggestTopic()
        {
            if (!IsFormValid)
                throw new InvalidOperationException("Invalid form can't be submitted.");

            if (!_assemblyService.CanChange(_assemblyId))
            {
                ShowErrorAlert();
                return;
            }

            _topicService.SuggestTopic(_assemblyId, _residentId, Description);

            Description = string.Empty;
            UpdateTopics();

            ShowSuccessAlert();
        }

        private void ShowErrorAlert()
            => ShowAlert(
                    Alert.Create(
                        "Grеška",
                        "Teme je moguće mijenjati najkasnije 24 sata prije početka skupštine.",
                        AlertType.Error
                    )
                    .AddOption("U redu")
                );

        private void ShowSuccessAlert()
            => ShowAlert(
                Alert.Create(
                    "Tema predložena",
                    "Uspješno ste predložili temu za skupštinu",
                    AlertType.Info
                )
                .AddOption("U redu")
            );

        private void RemoveTopic(AssemblyTopicDto? topic)
        {
            if (topic is null) return;

            if (!_assemblyService.CanChange(_assemblyId))
            {
                ShowErrorAlert();
                return;
            }

            ShowAlert(
                Alert.Create(
                    "Upozorenje",
                    "Da li ste sigurni da želite da uklonite dodanu temu?",
                    AlertType.Warning
                )
                .AddOption("Otkaži")
                .AddOption(
                    "Potvrdi",
                    () => {
                        _topicService.DeleteTopic(topic);
                        UpdateTopics();
                    }
                )
            );
        }

        private string _description = string.Empty;
        public string Description
        {
            get => _description;
            set
            {
                if (_description == value) return;
                _description = value;
                OnPropertyChanged(nameof(Description));
                OnPropertyChanged(nameof(IsFormValid));
                SuggestTopicCommand.RaiseCanExecuteChanged();
            }
        }

        public bool IsFormValid => !string.IsNullOrWhiteSpace(Description);
    }
}
