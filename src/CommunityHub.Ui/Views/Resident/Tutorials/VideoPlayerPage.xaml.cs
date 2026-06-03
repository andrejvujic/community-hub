using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident.Tutorials;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Tutorials
{
    public partial class VideoPlayerPage : MainNavigationConsumerPage
    {
        public VideoPlayerPage(string title, string videoPath)
        {
            InitializeComponent();
            DataContext = new VideoPlayerViewModel(title, videoPath, this);
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e)
            => Pop();
    }
}
