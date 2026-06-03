using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.Views.Resident.Tutorials;
using System.Collections.ObjectModel;

namespace CommunityHub.Ui.ViewModels.Resident.Tutorials
{
    public class TutorialVideo
    {
        public string Title { get; }
        public string Path { get; }

        public TutorialVideo(string title, string path)
        {
            Title = title;
            Path = path;
        }   
    }

    public class TutorialsViewModel : BaseViewModel
    {
        public TutorialsViewModel(IMainNavigationConsumer navigationConsumer) : base(navigationConsumer)
        {
            OpenVideoPlayerCommand = new(OpenVideoPlayer);
        }

        public ObservableCollection<TutorialVideo> TutorialVideos { get; set; } = new() {
            new("Kako da se pridružim svojoj zgradi?", "/Resources/Videos/Tutorial_0.mp4"),
            new("Kako da objavim oglas?", "/Resources/Videos/Tutorial_1.mp4"),
            new("Kako funkcionišu skupštine stanara?", "/Resources/Videos/Tutorial_2.mp4"),
        };

        public RelayCommand<TutorialVideo> OpenVideoPlayerCommand { get; }

        private void OpenVideoPlayer(TutorialVideo? video)
        {
            if (video is null) return;
            Push(new VideoPlayerPage(video.Title, video.Path));
        }
    }
}
