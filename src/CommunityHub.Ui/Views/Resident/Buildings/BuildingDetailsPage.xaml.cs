using CommunityHub.Application.Domain;
using CommunityHub.Ui.Helpers.Navigation;
using System.ComponentModel;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings
{
    public partial class BuildingDetailsPage : MainNavigatorConsumerPage, INotifyPropertyChanged
    {
        public Building Building { get; }

        private int CurrentImageIndex { get; set; } = 0;
        public Image? CurrentImage
        {
            get
            {
                if (CurrentImageIndex >= 0 && CurrentImageIndex < Building.Images.Count)
                    return Building.Images[CurrentImageIndex];
                return null;
            }
        }

        public BuildingDetailsPage(Building building)
        {
            InitializeComponent();
            DataContext = this;
            Building = building;
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e) => Pop();

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void PreviousImageButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (HasPreviousImage)
            {
                CurrentImageIndex--;
                OnPropertyChanged(nameof(CurrentImage));
                OnPropertyChanged(nameof(HasPreviousImage));
                OnPropertyChanged(nameof(HasNextImage));
            }
        }

        private void NextImageButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (HasNextImage)
            {
                CurrentImageIndex++;
                OnPropertyChanged(nameof(CurrentImage));
                OnPropertyChanged(nameof(HasPreviousImage));
                OnPropertyChanged(nameof(HasNextImage));
            }
        }

        public bool HasPreviousImage { get => CurrentImageIndex > 0; }
        public bool HasNextImage { get => CurrentImageIndex < Building.Images.Count - 1; }
    }
}
