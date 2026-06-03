using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Helpers.Navigation;
using System.IO;

namespace CommunityHub.Ui.ViewModels.Resident.Tutorials
{
    public class VideoPlayerViewModel : BaseViewModel
    {
        public VideoPlayerViewModel(
            string title,
            string videoPath,
            IMainNavigationConsumer navigationConsumer) : base(navigationConsumer)
        {
            Title = title;
            VideoPath = videoPath;
            PauseCommand = new(Pause, () => IsLoaded && IsPlaying);
            UnpauseCommand = new(Unpause, () => IsLoaded && !IsPlaying);
        }

        public string Title { get; }
        public string VideoPath { get; }
        public Uri VideoUri => new(GetAbsoluteVideoFilePath(VideoPath), UriKind.Absolute);
        public RelayCommand PauseCommand { get; }
        public RelayCommand UnpauseCommand { get; }
        public Action? PauseVideo { get; set; }
        public Action? UnpauseVideo { get; set; }

        private bool _isLoaded;
        public bool IsLoaded
        {
            get => _isLoaded;
            private set
            {
                if (_isLoaded == value) return;
                _isLoaded = value;
                OnPropertyChanged(nameof(IsLoaded));
                RaisePlaybackCanExecuteChanged();
            }
        }

        private bool _isPlaying;
        public bool IsPlaying
        {
            get => _isPlaying;
            private set
            {
                if (_isPlaying == value) return;
                _isPlaying = value;
                OnPropertyChanged(nameof(IsPlaying));
                RaisePlaybackCanExecuteChanged();
            }
        }

        private void Pause()
        {
            PauseVideo?.Invoke();
            IsPlaying = false;
        }

        private void Unpause()
        {
            Play();
        }

        public void Play()
        {
            if (!IsLoaded) return;
            UnpauseVideo?.Invoke();
            IsPlaying = true;
        }

        public void Stop()
        {
            IsPlaying = false;
        }

        public void MarkLoadedAndPlaying()
        {
            IsLoaded = true;
            IsPlaying = true;
        }

        private void RaisePlaybackCanExecuteChanged()
        {
            PauseCommand.RaiseCanExecuteChanged();
            UnpauseCommand.RaiseCanExecuteChanged();
        }

        private static string GetAbsoluteVideoFilePath(string videoPath)
            => Path.Combine(
                AppContext.BaseDirectory,
                videoPath
                    .TrimStart('/', '\\')
                    .Replace('/', Path.DirectorySeparatorChar)
                    .Replace('\\', Path.DirectorySeparatorChar)
            );
    }
}
