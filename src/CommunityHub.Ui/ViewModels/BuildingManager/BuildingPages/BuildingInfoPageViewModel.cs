using CommunityHub.Application.Domain.Users;
using CommunityHub.Application.Domain.Buildings;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using CommunityHub.Application.Services.Dtos.Buildings;
using CommunityHub.Application.MappersDto.Buildings;
using System.Windows.Input;
using CommunityHub.Ui.Helpers.Commands;

namespace CommunityHub.Ui.ViewModels.BuildingManager.BuildingPages
{
    public class BuildingInfoPageViewModel : INotifyPropertyChanged
    {
        private readonly BuildingMapper _buildingMapper = new();
        public User User { get; }
        public Building Building { get; }
        public BuildingDto BuildingDto { get; }
        public BuildingInfoPageViewModel(User user, Building building)
        {
            User = user;
            Building = building;
            BuildingDto = _buildingMapper.MapToDto(building);
            PreviousImageCommand = new RelayCommand<object?>(_ => PreviousImage());
            NextImageCommand = new RelayCommand<object?>(_ => NextImage());
        }

        private int _currentImageIndex = 0;
        public int CurrentImageIndex
        {
            get => _currentImageIndex;
            set
            {
                if (_currentImageIndex != value)
                {
                    _currentImageIndex = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CurrentImage));
                    OnPropertyChanged(nameof(ImageCounterText));
                }
            }
        }

        public BitmapImage? CurrentImage
        {
            get
            {
                if (BuildingDto.Images == null || BuildingDto.Images.Count == 0)
                    return null;

                string path = BuildingDto.Images[CurrentImageIndex].Path;
                return new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
            }
        }
        public string ImageCounterText
        {
            get
            {
                if (BuildingDto.Images == null || BuildingDto.Images.Count == 0)
                    return "0 / 0";

                return $"{CurrentImageIndex + 1} / {BuildingDto.Images.Count}";
            }
        }
        
        public ICommand PreviousImageCommand { get; }
        public ICommand NextImageCommand { get; }

        public void NextImage()
        {
            if (BuildingDto.Images != null && BuildingDto.Images.Count > 0)
            {
                if(CurrentImageIndex + 1 >= BuildingDto.Images.Count)
                    CurrentImageIndex = 0;
                else
                    CurrentImageIndex++;
            }
        }

        public void PreviousImage()
        {
            if (BuildingDto.Images != null && BuildingDto.Images.Count > 0)
            {
                if(CurrentImageIndex - 1 < 0)
                    CurrentImageIndex = BuildingDto.Images.Count - 1;
                else
                    CurrentImageIndex--;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}