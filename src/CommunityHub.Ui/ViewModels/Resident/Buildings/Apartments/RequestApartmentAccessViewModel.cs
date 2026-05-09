using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.Apartments.AccessRequests;
using CommunityHub.Application.Domain.Buildings;
using CommunityHub.Application.Modules.Buildings.Core.Services.Apartments;
using System.ComponentModel;

namespace CommunityHub.Ui.ViewModels.Resident.Buildings.Apartments
{
    public class RequestApartmentAccessViewModel : INotifyPropertyChanged
    {
        private readonly ApartmentAccessRequestService _apartmentAccessRequestService = new();
        private readonly ApartmentService _apartmentService = new();

        public long UserId { get; set; }
        public Building Building { get; }

        public RequestApartmentAccessViewModel(long userId, Building building) {
            UserId = userId;
            Building = building;
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

        public AccessRequestValidationResult Submit()
        {
            var result = ValidateAccessRequest();
            if (result is AccessRequestValidationResult.Valid)
                CreateAccessRequest();
            return result;
        }

        public AccessRequestValidationResult ValidateAccessRequest()
            => _apartmentAccessRequestService.ValidateAccessRequest(
                    UserId,
                    Building.Id,
                    ApartmentNumber
                );

        public void CreateAccessRequest()
        {
            var apartmentId = _apartmentService.FindIdByBuildingAndNumber(
                Building.Id,
                ApartmentNumber
            );

            if (apartmentId is null)
                throw new ArgumentException("ApartmentId can't be null.");

            _apartmentAccessRequestService.Create(
                new ApartmentAccessRequest(
                    applicantId: UserId,
                    apartmentId: apartmentId!.Value,
                    status: Status.Waiting,
                    DateTime.Now
                )
            );
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
