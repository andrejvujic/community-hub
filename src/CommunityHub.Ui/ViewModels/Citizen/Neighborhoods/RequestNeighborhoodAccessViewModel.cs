using System.ComponentModel;

namespace CommunityHub.Ui.ViewModels.Citizen.Neighborhoods
{
    public class RequestNeighborhoodAccessViewModel : INotifyPropertyChanged
    {
        private readonly string _neighborhoodName;

        public string NeighborhoodName => _neighborhoodName;

        public bool Confirmed { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public RequestNeighborhoodAccessViewModel(string neighborhoodName)
        {
            _neighborhoodName = neighborhoodName;
        }

        protected virtual void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
