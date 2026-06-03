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
        }

        public string Title { get; }
        public string VideoPath { get; }
        public Uri VideoUri => new(GetAbsoluteVideoFilePath(VideoPath), UriKind.Absolute);


        private double _durationSeconds;
        public double DurationSeconds
        {
            get => _durationSeconds;
            private set
            {
                _durationSeconds = value;
                OnPropertyChanged(nameof(DurationSeconds));
            }
        }

        private double _positionSeconds;
        public double PositionSeconds
        {
            get => _positionSeconds;
            set
            {
                _positionSeconds = value;
                OnPropertyChanged(nameof(PositionSeconds));
            }
        }

        private bool _isLoaded;
        public bool IsLoaded
        {
            get => _isLoaded;
            private set
            {
                if (_isLoaded == value) return;
                _isLoaded = value;
                OnPropertyChanged(nameof(IsLoaded));
                OnPropertyChanged(nameof(CanPlay));
                OnPropertyChanged(nameof(CanPause));
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
                OnPropertyChanged(nameof(CanPlay));
                OnPropertyChanged(nameof(CanPause));
            }
        }

        private bool _areControlsDisabled;
        public bool AreControlsDisabled
        {
            get => _areControlsDisabled;
            private set
            {
                if (_areControlsDisabled == value) return;
                _areControlsDisabled = value;
                OnPropertyChanged(nameof(AreControlsDisabled));
                OnPropertyChanged(nameof(CanPlay));
                OnPropertyChanged(nameof(CanPause));
            }
        }

        public void Stop()
        {
            if (!IsLoaded || !IsPlaying) return;
            IsPlaying = false;
        }

        public void Play()
        {
            if (!IsLoaded || IsPlaying) return;
            IsPlaying = true;
        }

        public void MarkLoadedAndPlaying(TimeSpan duration)
        {
            IsLoaded = true;
            IsPlaying = true;
            DurationSeconds = duration.TotalSeconds;
        }

        public bool CanPlay => IsLoaded && !IsPlaying && !AreControlsDisabled;
        public bool CanPause => IsLoaded && IsPlaying && !AreControlsDisabled;

        public void UpdatePosition(TimeSpan position)
        {
            PositionSeconds = position.TotalSeconds;
        }

        public void DisableControls() => AreControlsDisabled = true;
        public void EnableControls() => AreControlsDisabled = false;

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
