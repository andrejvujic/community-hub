using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Helpers.Navigation;

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
            PauseCommand = new(Pause);
            UnpauseCommand = new(Unpause);
        }

        public string Title { get; }
        public string VideoPath { get; }
        public RelayCommand PauseCommand { get; }
        public RelayCommand UnpauseCommand { get; }

        private void Pause()
        {
        }

        private void Unpause()
        {
        }
    }
}
