using CommunityHub.Application.Domain;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace CommunityHub.Ui.Views.BuildingManager
{
    public partial class BuildingInfoPage : Page
    {
        private readonly User _user;
        private readonly Building _building;
        private int currentImage = 0;
        public BuildingInfoPage(User user, Building building)
        {
            InitializeComponent();
            _user = user;
            _building = building;
            RefreshImage();
            DataContext = _building;
            
        }
        private void LeftArrow_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var homeWindow = Window.GetWindow(this) as HomeWindow;

            if (homeWindow != null)
                homeWindow.HomeFrame.Navigate(new MyBuildingsPage(_user));
        }

        private void RefreshImage()
        {
            string path = _building.Images[currentImage].Path;
            displayedImage.Source = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
            counterTextBlock.Text = $"{currentImage + 1} / {_building.Images.Count}";
        }
        private void PreviousImage_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(currentImage - 1 < 0)
                currentImage = _building.Images.Count - 1;
            else
                currentImage--;

            RefreshImage();
        }

        private void NextImage_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(currentImage + 1 >= _building.Images.Count)
                currentImage = 0;
            else
                currentImage++;

            RefreshImage();
        }
    }
}