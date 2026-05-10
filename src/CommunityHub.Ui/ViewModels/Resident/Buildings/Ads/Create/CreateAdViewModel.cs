using CommunityHub.Application;
using CommunityHub.Application.Domain.Ads;
using CommunityHub.Application.Domain.Ads.RepositoryInterfaces;
using CommunityHub.Application.Domain.Notifications.RepositoryInterfaces;
using CommunityHub.Application.Modules.Buildings.Core.Services.Ads;
using System.ComponentModel;
using System.Globalization;

namespace CommunityHub.Ui.ViewModels.Resident.Buildings.Ads.Create
{
    public class SubmissionResult
    {
        public PublishmentValidationResult ValidationResult { get; }

        private readonly Ad? _ad;
        public Ad PublishedAd => _ad ?? throw new InvalidOperationException("Submission wasn't successful.");

        public SubmissionResult(PublishmentValidationResult validationResult, Ad? ad = null)
        {
            ValidationResult = validationResult;
            _ad = ad;
        }

        public bool IsSuccessful => ValidationResult is PublishmentValidationResult.Valid && _ad is not null;
    }

    public class CreateAdViewModel: INotifyPropertyChanged
    {
        private readonly CultureInfo _culture = new("sr-Latn-RS");
        private readonly DateTime _today = DateTime.Today;

        private readonly AdService _adService;
        private readonly AdMatchSubscriptionService _adMatchSubscriptionService;

        public long UserId { get; }
        public long BuildingId { get; }

        public CreateAdViewModel(long userId, long buildingId)
        {
            _adService = new(
                Injector.CreateInstance<IAdRepository>(),
                Injector.CreateInstance<IAdTimeSlotRepository>()
            );

            _adMatchSubscriptionService = new(
                Injector.CreateInstance<IAdRepository>(),
                Injector.CreateInstance<IAdMatchSubscriptionRepository>(),
                Injector.CreateInstance<IAdTimeSlotRepository>(),
                Injector.CreateInstance<INotificationRepository>()
            );

            UserId = userId;
            BuildingId = buildingId;
        }

        public SubmissionResult Submit()
        {
            if (!IsFormValid)
                throw new InvalidOperationException("Invalid form can't be submitted.");

            var ad = new Ad(
                authorId: UserId,
                buildingId: BuildingId,
                type: SelectedType!.Value,
                category: SelectedCategory!.Value,
                status: AdStatus.Active,
                description: Description,
                startDate: SelectedStartDate!.Value,
                endDate: SelectedEndDate!.Value
            );

            var validationResult = _adService.ValidatePublishment(ad);

            if (validationResult is PublishmentValidationResult.Valid)
            {
                var publishedAd = _adService.Publish(ad);
                _adMatchSubscriptionService.NotifyAuthorsOfSubscribedMatchingAds(publishedAd);
                return new(validationResult, publishedAd);
            }

            return new(validationResult);
        }

        private AdType? _selectedType;
        public AdType? SelectedType
        {
            get => _selectedType;
            set
            {
                if (_selectedType == value) return;
                _selectedType = value;
                OnPropertyChanged(nameof(SelectedType));
                OnPropertyChanged(nameof(IsFormValid));
            }
        }

        private AdCategory? _selectedCategory;
        public AdCategory? SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (_selectedCategory == value) return;
                _selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
                OnPropertyChanged(nameof(IsFormValid));
            }
        }

        private string _description = string.Empty;
        public string Description
        {
            get => _description;
            set
            {
                if (_description == value) return;
                _description = value;
                OnPropertyChanged(nameof(Description));
                OnPropertyChanged(nameof(IsFormValid));
            }
        }

        public DateTime MinStartDate => _today;
        public DateTime? MaxStartDate => SelectedEndDate;
        public DateTime MinEndDate => SelectedStartDate ?? _today;


        private DateTime? _selectedStartDate;
        public DateTime? SelectedStartDate
        {
            get => _selectedStartDate;
            set
            {
                if (_selectedStartDate == value) return;
                _selectedStartDate = value is null ? null : value.Value.Date;

                OnPropertyChanged(nameof(SelectedStartDate));
                OnPropertyChanged(nameof(SelectedStartDateText));
                OnPropertyChanged(nameof(SelectedDateRangeText));

                OnPropertyChanged(nameof(MinEndDate));

                OnPropertyChanged(nameof(IsSelectedDateRangeValid));
                OnPropertyChanged(nameof(IsFormValid));
            }
        }

        public string SelectedStartDateText
            => SelectedStartDate is null ?
                "Izaberite..." :
                SelectedStartDate.Value.ToString("d. MMMM", _culture);


        private DateTime? _selectedEndDate;
        public DateTime? SelectedEndDate
        {
            get => _selectedEndDate;
            set
            {
                if (_selectedEndDate == value) return;
                _selectedEndDate = value is null ? null : value.Value.Date;

                OnPropertyChanged(nameof(SelectedEndDate));
                OnPropertyChanged(nameof(SelectedEndDateText));
                OnPropertyChanged(nameof(SelectedDateRangeText));

                OnPropertyChanged(nameof(MaxStartDate));

                OnPropertyChanged(nameof(IsSelectedDateRangeValid));
                OnPropertyChanged(nameof(IsFormValid));
            }
        }

        public string SelectedEndDateText
            => SelectedEndDate is null ?
                "Izaberite..." :
                SelectedEndDate.Value.ToString("d. MMMM", _culture);

        public string SelectedDateRangeText
        {
            get
            {
                if (SelectedStartDate is null || SelectedEndDate is null) return "Izaberite...";
                return $"{SelectedStartDateText} — {SelectedEndDateText}";
            }
        }

        public bool IsSelectedDateRangeValid
        {
            get
            {
                if (SelectedStartDate is null || SelectedEndDate is null) return false;
                if (SelectedStartDate.Value < _today) return false;
                if (SelectedEndDate.Value < SelectedStartDate.Value) return false;

                return true;
            }
        }

        public bool IsFormValid
        {
            get
            {
                if (!IsSelectedDateRangeValid) return false;

                if (string.IsNullOrWhiteSpace(Description)) return false;

                if (SelectedType is null || SelectedCategory is null) return false;

                return true;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
