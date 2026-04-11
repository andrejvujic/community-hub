using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;
using CommunityHub.Ui.Helpers.Navigation;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.BuildingResident
{
    public enum AccessRequestValidationResult
    {
        Valid,
        ApartmentNotFound,
        AlreadyResident,
        ApartmentOccupied
    }

    public partial class RequestApartmentAccessPage : MainNavigatorConsumerPage, INotifyPropertyChanged
    {
        private readonly long _userId;

        public Building Building { get; }

        private readonly UserDbRepository _userRepository;
        private readonly ApartmentDbRepository _apartmentRepository;
        private readonly ApartmentAccessRequestDbRepository _apartmentAccessRequestRepository;
        private readonly ApartmentMembershipDbRepository _apartmentMembershipRepository;

        public RequestApartmentAccessPage(long userId, Building building)
        {
            InitializeComponent();
            DataContext = this;

            _userId = userId;
            Building = building;

            _userRepository = new UserDbRepository();
            _apartmentRepository = new ApartmentDbRepository();
            _apartmentAccessRequestRepository = new ApartmentAccessRequestDbRepository();
            _apartmentMembershipRepository = new ApartmentMembershipDbRepository();
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
            if (string.IsNullOrWhiteSpace(ApartmentNumberText))
            {
                ApartmentNumberInvalidError.Visibility = Visibility.Collapsed;
                return;
            }

            ApartmentNumberInvalidError.Visibility = ApartmentNumber > 0 ? Visibility.Collapsed : Visibility.Visible;
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e) => Pop();

        private AccessRequestValidationResult ValidateAccessRequest(long? apartmentId)
        {
            if (apartmentId == null)
                return AccessRequestValidationResult.ApartmentNotFound;

            if (_apartmentMembershipRepository.IsResidentOfApartment(_userId, apartmentId.Value))
                return AccessRequestValidationResult.AlreadyResident;

            if (_apartmentMembershipRepository.IsApartmentOccupied(apartmentId.Value))
                return AccessRequestValidationResult.ApartmentOccupied;

            return AccessRequestValidationResult.Valid;
        }

        private void ShowAccessRequestValidationResult(AccessRequestValidationResult result)
        {
            switch (result)
            {
                case AccessRequestValidationResult.ApartmentNotFound:
                    ApartmentNotFoundAlert.Visibility = Visibility.Visible;
                    break;
                case AccessRequestValidationResult.ApartmentOccupied:
                    ApartmentOccupiedAlert.Visibility = Visibility.Visible;
                    break;
                case AccessRequestValidationResult.AlreadyResident:
                    AlreadyResidentAlert.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void CreateAccessRequest(long? apartmentId) {
            if (apartmentId == null) return;

            var apartment = _apartmentRepository.GetById(apartmentId.Value);
            var applicant = _userRepository.GetById(_userId);
            if (apartment == null || applicant == null) return;

            var request = new ApartmentAccessRequest(
                applicant,
                apartment,
                Status.Waiting,
                DateTime.Now
            );

            _apartmentAccessRequestRepository.Create(request);
            PopAll();   
        }

        private void CreateAccessRequestButton_Click(object sender, RoutedEventArgs e)
        {
            long? apartmentId = _apartmentRepository.FindIdByBuildingAndNumber(Building.Id, ApartmentNumber);

            AccessRequestValidationResult result = ValidateAccessRequest(apartmentId);

            if (result == AccessRequestValidationResult.Valid)
            {
                CreateAccessRequest(apartmentId);
                return;
            }

            ShowAccessRequestValidationResult(result);
        }

        private void CreateAccessRequestAlthoughOccupiedButton_Click(object sender, RoutedEventArgs e)
        {
            ApartmentOccupiedAlert.Visibility= Visibility.Collapsed;

            long? apartmentId = _apartmentRepository.FindIdByBuildingAndNumber(Building.Id, ApartmentNumber);
            CreateAccessRequest(apartmentId);
        }

        private void CloseAparmentNotFoundAlertButton_Click(object sender, RoutedEventArgs e)
            => ApartmentNotFoundAlert.Visibility = Visibility.Collapsed;

        private void CloseApartmentOccupiedAlertButton_Click(object sender, RoutedEventArgs e)
            => ApartmentOccupiedAlert.Visibility = Visibility.Collapsed;

        private void CloseAlreadyResidentAlertButton_Click(object sender, RoutedEventArgs e)
            => AlreadyResidentAlert.Visibility = Visibility.Collapsed;


        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
