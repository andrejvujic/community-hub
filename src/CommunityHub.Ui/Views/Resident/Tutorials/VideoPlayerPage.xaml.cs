using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident.Tutorials;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Tutorials
{
    public partial class VideoPlayerPage : MainNavigationConsumerPage
    {
        private readonly VideoPlayerViewModel _viewModel;

        public VideoPlayerPage(string title, string videoPath)
        {
            InitializeComponent();
            _viewModel = new VideoPlayerViewModel(title, videoPath, this)
            {
                PauseVideo = () => VideoElement.Pause(),
                UnpauseVideo = () => VideoElement.Play()
            };
            DataContext = _viewModel;

            Loaded += VideoPlayerPage_Loaded;
            Unloaded += VideoPlayerPage_Unloaded;
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e)
            => Pop();

        private void VideoPlayerPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            VideoElement.Play();
        }

        private void VideoElement_MediaOpened(object sender, System.Windows.RoutedEventArgs e)
        {
            _viewModel.MarkLoadedAndPlaying();
        }

        private void VideoPlayerPage_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            VideoElement.Stop();
            _viewModel.Stop();
            Loaded -= VideoPlayerPage_Loaded;
            Unloaded -= VideoPlayerPage_Unloaded;
        }
    }
}
