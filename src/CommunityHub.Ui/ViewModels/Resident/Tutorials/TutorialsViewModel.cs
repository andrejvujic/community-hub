using System.Collections.ObjectModel;

namespace CommunityHub.Ui.ViewModels.Resident.Tutorials
{
    public class TutorialVideo
    {
        public string Title { get; set; }
        public string Path { get; set; }

        public TutorialVideo(string title, string path)
        {
            Title = title;
            Path = path;
        }   
    }

    public class TutorialsViewModel
    {
        public ObservableCollection<TutorialVideo> TutorialVideos { get; set; } = new() {
            new("Kako da se pridružim svojoj zgradi?", "Resources/Videos/Tutorial_0.mp4"),
            new("Kako da objavim oglas?", "Resources/Videos/Tutorial_1.mp4"),
            new("Kako funkcionišu skupštine stanara?", "Resources/Videos/Tutorial_2.mp4"),
        };
    }
}
    