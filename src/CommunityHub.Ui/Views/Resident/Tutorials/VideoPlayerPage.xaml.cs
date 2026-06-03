using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident.Tutorials;
using System.Windows.Input;
using System.Windows.Threading;

namespace CommunityHub.Ui.Views.Resident.Tutorials
{
    public partial class VideoPlayerPage : MainNavigationConsumerPage
    {
        private readonly VideoPlayerViewModel _viewModel;
        private readonly DispatcherTimer _progressTimer = new();
        private bool _isUpdatingProgress;

        public VideoPlayerPage(string title, string videoPath)
        {
            InitializeComponent();
            _viewModel = new VideoPlayerViewModel(title, videoPath, this)
            {
                PauseVideo = PauseVideo,
                UnpauseVideo = PlayVideo
            };
            DataContext = _viewModel;

            _progressTimer.Interval = TimeSpan.FromMilliseconds(250);
            _progressTimer.Tick += ProgressTimer_Tick;

            Loaded += VideoPlayerPage_Loaded;
            Unloaded += VideoPlayerPage_Unloaded;
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e)
            => Pop();

        private void VideoPlayerPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            PlayVideo();
        }

        private void VideoElement_MediaOpened(object sender, System.Windows.RoutedEventArgs e)
        {
            var duration = VideoElement.NaturalDuration.HasTimeSpan
                ? VideoElement.NaturalDuration.TimeSpan
                : TimeSpan.Zero;

            _viewModel.MarkLoadedAndPlaying(duration);
            _progressTimer.Start();
        }

        private void PlayVideo()
        {
            VideoElement.Play();
            if (_viewModel.IsLoaded)
                _progressTimer.Start();
        }

        private void PauseVideo()
        {
            VideoElement.Pause();
            _progressTimer.Stop();
        }

        private void ProgressTimer_Tick(object? sender, EventArgs e)
        {
            _isUpdatingProgress = true;
            _viewModel.UpdatePosition(VideoElement.Position);
            _isUpdatingProgress = false;
        }

        private void ProgressSlider_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {
            if (_isUpdatingProgress || !_viewModel.IsLoaded)
                return;

            VideoElement.Position = TimeSpan.FromSeconds(e.NewValue);
        }

        private void VideoPlayerPage_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _progressTimer.Stop();
            _progressTimer.Tick -= ProgressTimer_Tick;
            VideoElement.Stop();
            _viewModel.Stop();
            Loaded -= VideoPlayerPage_Loaded;
            Unloaded -= VideoPlayerPage_Unloaded;
        }
    }
}
