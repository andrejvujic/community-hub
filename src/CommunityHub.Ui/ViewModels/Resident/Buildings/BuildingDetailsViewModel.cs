using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.Buildings;
using System.ComponentModel;

namespace CommunityHub.Ui.ViewModels.Resident.Buildings
{
    public class BuildingDetailsViewModel : INotifyPropertyChanged
    {
        public Building Building { get; }

        public BuildingDetailsViewModel(Building building)
        {
            Building = building;
        }

        private int _currentImageIndex = 0;
        public int CurrentImageIndex
        {
            get => _currentImageIndex; 
            set
            {
                if (_currentImageIndex == value) return;
                _currentImageIndex = value;
                OnPropertyChanged(nameof(CurrentImageIndex));
                OnPropertyChanged(nameof(CurrentImage));
                OnPropertyChanged(nameof(CurrentImageText));
                OnPropertyChanged(nameof(HasPreviousImage));
                OnPropertyChanged(nameof(HasNextImage));
            }
        }

        public Image? CurrentImage
        {
            get
            {
                if (CurrentImageIndex >= 0 && CurrentImageIndex < Building.Images.Count)
                    return Building.Images[CurrentImageIndex];
                return null;
            }
        }

        public string CurrentImageText => $"{CurrentImageIndex + 1} / {Building.Images.Count}";

        private const int textIndicatorThreshold = 10;
        public bool ShouldUseTextIndicator => Building.Images.Count > textIndicatorThreshold;

        public void GoToPreviousImage()
        {
            if (HasPreviousImage)
                --CurrentImageIndex;
        }

        public void GoToNextImage()
        {
            if (HasNextImage)
                ++CurrentImageIndex;
        }

        public bool HasPreviousImage { get => CurrentImageIndex > 0; }
        public bool HasNextImage { get => CurrentImageIndex < Building.Images.Count - 1; }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
