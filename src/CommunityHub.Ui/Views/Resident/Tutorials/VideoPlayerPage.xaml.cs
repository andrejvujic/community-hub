using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident.Tutorials;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Threading;

namespace CommunityHub.Ui.Views.Resident.Tutorials
{
    public partial class VideoPlayerPage : MainNavigationConsumerPage
    {
        private readonly VideoPlayerViewModel _viewModel;
        private readonly DispatcherTimer _progressTimer = new();
        private readonly DispatcherTimer _resumePlayingTimer = new();
        private bool _isUpdatingProgress = false;




        public VideoPlayerPage(string title, string videoPath)
        {
            InitializeComponent();
            _viewModel = new VideoPlayerViewModel(
                title: title,
                videoPath: videoPath, 
                navigationConsumer: this
            );
            DataContext = _viewModel;

            _progressTimer.Interval = TimeSpan.FromMilliseconds(250);
            _progressTimer.Tick += ProgressTimer_Tick;

            _resumePlayingTimer.Interval = TimeSpan.FromMilliseconds(500);
            _resumePlayingTimer.Tick += ResumePlayingTimer_Tick;

            Loaded += VideoPlayerPage_Loaded;
            Unloaded += VideoPlayerPage_Unloaded;
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e)
            => Pop();

        private void VideoPlayerPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            VideoElement.Play();
            if (_viewModel.IsLoaded)
                _progressTimer.Start();
        }

        private void VideoElement_MediaOpened(object sender, System.Windows.RoutedEventArgs e)
        {
            var duration = VideoElement.NaturalDuration.HasTimeSpan
                ? VideoElement.NaturalDuration.TimeSpan
                : TimeSpan.Zero;

            _viewModel.MarkLoadedAndPlaying(duration);
            _progressTimer.Start();
        }

        private void VideoElement_MediaEnded(object sender, System.Windows.RoutedEventArgs e)
        {
            VideoElement.Stop();
            _viewModel.Stop();
        }

        private void ProgressTimer_Tick(object? sender, EventArgs e)
        {
            _isUpdatingProgress = true;
            _viewModel.UpdatePosition(VideoElement.Position);
            _isUpdatingProgress = false;
        }

        private void ResumePlayingTimer_Tick(object? sender, EventArgs e)
        {
            _resumePlayingTimer.Stop();

            _viewModel.EnableControls();
            _viewModel.Play();

            VideoElement.Play();    
        }

        private void ProgressSlider_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {
            if (_isUpdatingProgress || !_viewModel.IsLoaded)
                return;

            if (_resumePlayingTimer.IsEnabled)
                _resumePlayingTimer.Stop();

            _viewModel.Stop();
            _viewModel.DisableControls();

            VideoElement.Pause();
            VideoElement.Position = TimeSpan.FromSeconds(e.NewValue);

            _resumePlayingTimer.Start();
        }

        private void VideoPlayerPage_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _progressTimer.Stop();
            _progressTimer.Tick -= ProgressTimer_Tick;
                
            if (_resumePlayingTimer.IsEnabled)
            {
                _resumePlayingTimer.Stop();
                _resumePlayingTimer.Tick -= ResumePlayingTimer_Tick;
            }

            VideoElement.Stop();
            _viewModel.Stop();

            Loaded -= VideoPlayerPage_Loaded;
            Unloaded -= VideoPlayerPage_Unloaded;
        }

        private void PauseButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!_viewModel.IsLoaded || !_viewModel.IsPlaying) return;
            VideoElement.Pause();
            _viewModel.Stop();
        }

        private void PlayButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!_viewModel.IsLoaded || _viewModel.IsPlaying) return;
            VideoElement.Play();
            _viewModel.Play();
        }

        private void VideoElement_FocusableChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is bool isFocusable && isFocusable)
                Debug.Write(isFocusable);
        }
    }
}
