using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;
using CommunityHub.Ui.Helpers.Alerts;
using CommunityHub.Ui.Helpers.Navigation;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Apartments
{
    public enum AccessRequestValidationResult
    {
        Valid,
        ApartmentNotFound,
        ApartmentOccupied,
        AlreadyMember,
        PendingRequestExists
    }

    public partial class RequestApartmentAccessPage : MainNavigatorConsumerPage, IAlertConsumer, INotifyPropertyChanged
    {
        private readonly long _userId;

        public Building Building { get; }

        private readonly UserRepository _userRepository;
        private readonly ApartmentRepository _apartmentRepository;
        private readonly ApartmentAccessRequestRepository _apartmentAccessRequestRepository;
        private readonly ApartmentMembershipRepository _apartmentMembershipRepository;

        public RequestApartmentAccessPage(long userId, Building building)
        {
            InitializeComponent();
            DataContext = this;

            _userId = userId;
            Building = building;

            _userRepository = new UserRepository();
            _apartmentRepository = new ApartmentRepository();
            _apartmentAccessRequestRepository = new ApartmentAccessRequestRepository();
            _apartmentMembershipRepository = new ApartmentMembershipRepository();
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

        public long? ApartmentId { get; private set; }

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

        private AccessRequestValidationResult ValidateAccessRequest()
        {
            if (ApartmentId == null)
                return AccessRequestValidationResult.ApartmentNotFound;

            if (_apartmentAccessRequestRepository.HasPendingRequest(_userId, ApartmentId.Value))
                return AccessRequestValidationResult.PendingRequestExists;

            if (_apartmentMembershipRepository.HasMembership(_userId, ApartmentId.Value))
                return AccessRequestValidationResult.AlreadyMember;

            if (_apartmentMembershipRepository.IsApartmentOccupied(ApartmentId.Value))
                return AccessRequestValidationResult.ApartmentOccupied;

            return AccessRequestValidationResult.Valid;
        }

        private void ShowAccessRequestValidationResult(AccessRequestValidationResult result)
        {
            if (this is not IAlertConsumer consumer) return;

            switch (result)
            {
                case AccessRequestValidationResult.ApartmentNotFound:
                    consumer.ShowAlert(
                        Alert.Create(
                            "Greška",
                            $"Stan broj {ApartmentNumber} ne postoji u zgradi {Building.Address.Street.Name} {Building.Address.StreetNumber}. Provjerite uneseni broj stana i pokušajte ponovo.",
                            AlertType.Error
                        )
                        .AddOption("U redu")
                    );
                    break;

                case AccessRequestValidationResult.ApartmentOccupied:
                        consumer.ShowAlert(
                            Alert.Create(
                                "Upozorenje",
                                $"Da li ste sigurni da ste unijeli ispravan broj stana? Već postoji član koji je pridružen stanu broj {ApartmentNumber} u zgradi {Building.Address.Street.Name} {Building.Address.StreetNumber}.",
                                AlertType.Warning
                            )
                            .AddOption("Otkaži")
                            .AddOption("Nastavi", () => CreateAccessRequest())
                        );
                    break;

                case AccessRequestValidationResult.AlreadyMember:
                    consumer.ShowAlert(
                          Alert.Create(
                              "Greška",
                              $"Nije moguće kreirati zahtjev za pridruženje stanu broj {ApartmentNumber} u zgradi {Building.Address.Street.Name} {Building.Address.StreetNumber} zato što ste već učlanjeni u ovaj stan.",
                              AlertType.Error
                          )
                          .AddOption("U redu")
                      );
                    break;

                case AccessRequestValidationResult.PendingRequestExists:
                    consumer.ShowAlert(
                          Alert.Create(
                              "Greška",
                              $"Nije moguće kreirati zahtjev za pridruženje stanu broj {ApartmentNumber} u zgradi {Building.Address.Street.Name} {Building.Address.StreetNumber} zato što ste već imate zahtjev za ovaj stan koji je još uvijek u fazi obrade.",
                              AlertType.Error
                          )
                          .AddOption("U redu")
                      );
                    break;
            }
        }

        private void ShowSuccessAlert()
        {
            if (this is IAlertConsumer consumer)
                consumer.ShowAlert(
                    Alert.Create(
                        "Zahtjev podnesen",
                        "Vaš zahtjev je uspješno podnesen. Sve svoje zahtjeve možete pogledati na početnom ekranu.",
                        AlertType.Info
                    )
                    .AddOption("U redu", () => PopAll())
                );
        }

        private void CreateAccessRequest() {
            if (ApartmentId == null) return;

            var apartment = _apartmentRepository.GetById(ApartmentId.Value);
            var applicant = _userRepository.GetById(_userId);
            if (apartment == null || applicant == null) return;

            var request = new ApartmentAccessRequest(
                applicant,
                apartment,
                Status.Waiting,
                DateTime.Now
            );

            _apartmentAccessRequestRepository.Create(request);

            ShowSuccessAlert();
        }

        private void CreateAccessRequestButton_Click(object sender, RoutedEventArgs e)
        {
            ApartmentId = _apartmentRepository.FindIdByBuildingAndNumber(Building.Id, ApartmentNumber);

            AccessRequestValidationResult result = ValidateAccessRequest();

            if (result == AccessRequestValidationResult.Valid)
            {
                CreateAccessRequest();
                return;
            }

            ShowAccessRequestValidationResult(result);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
