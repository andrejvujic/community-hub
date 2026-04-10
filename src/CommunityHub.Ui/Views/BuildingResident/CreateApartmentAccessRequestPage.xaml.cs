using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;
using CommunityHub.Ui.Helpers.Navigation;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.BuildingResident
{
    public partial class CreateApartmentAccessRequestPage : MainNavigatorConsumerPage, INotifyPropertyChanged
    {
        public Building Building { get; }

        private readonly ApartmentDbRepository _apartmentDbRepository;
        private readonly ApartmentMembershipDbRepository _apartmentMembershipDbRepository;

        public CreateApartmentAccessRequestPage(Building building)
        {
            InitializeComponent();
            DataContext = this;

            Building = building;

            _apartmentDbRepository = new ApartmentDbRepository();
            _apartmentMembershipDbRepository = new ApartmentMembershipDbRepository();
        }

        public int ApartmentNumber
        {
            get
            {
                if (int.TryParse(ApartmentNumberText, out int apartmentNumber))
                    return apartmentNumber;
                return 0;
            }
        }

        public bool IsApartmentNumberValid { get => ApartmentNumber > 0; }

        private string _apartmentNumberText = string.Empty;
        public string ApartmentNumberText
        {
            get => _apartmentNumberText;
            set
            {
                if (value == _apartmentNumberText) return;
                _apartmentNumberText = value;
                OnPropertyChanged(nameof(ApartmentNumberText));
                OnPropertyChanged(nameof(ApartmentNumber));
                OnPropertyChanged(nameof(IsApartmentNumberValid));
            }
        }

        private void ApartmentNumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApartmentNumberInvalidError.Visibility = ApartmentNumber > 0 ? Visibility.Hidden : Visibility.Visible;
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e) => Pop();

        private void CreateRequestButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_apartmentDbRepository.Exists(Building.Id, ApartmentNumber))
            {
                ApartmentDoesntExistAlert.Visibility = Visibility.Visible;
                return;
            }

            if (_apartmentMembershipDbRepository.IsApartmentOccupied(Building.Id, ApartmentNumber))
            {
                ApartmentOccupiedAlert.Visibility = Visibility.Visible;
                return;
            }

        }

        private void CloseApartmentDoesntExistAlertButton_Click(object sender, RoutedEventArgs e)
            => ApartmentDoesntExistAlert.Visibility= Visibility.Collapsed;

        private void CloseApartmentOccupiedAlertButton_Click(object sender, RoutedEventArgs e)
            => ApartmentOccupiedAlert.Visibility = Visibility.Collapsed;


        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
