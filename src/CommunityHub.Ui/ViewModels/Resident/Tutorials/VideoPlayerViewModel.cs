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
            PauseCommand = new(Pause);
            UnpauseCommand = new(Unpause);
        }

        public string Title { get; }
        public string VideoPath { get; }
        public Uri VideoUri => new(GetAbsoluteVideoFilePath(VideoPath), UriKind.Absolute);
        public RelayCommand PauseCommand { get; }
        public RelayCommand UnpauseCommand { get; }
        public Action? PauseVideo { get; set; }
        public Action? UnpauseVideo { get; set; }

        private void Pause()
        {
            PauseVideo?.Invoke();
        }

        private void Unpause()
        {
            UnpauseVideo?.Invoke();
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
