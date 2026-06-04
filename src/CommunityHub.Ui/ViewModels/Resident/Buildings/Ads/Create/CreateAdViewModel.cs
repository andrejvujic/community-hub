using CommunityHub.Application;
using CommunityHub.Application.Domain.Ads;
using CommunityHub.Application.Domain.RepositoryInterfaces.Ads;
using CommunityHub.Application.Modules.Buildings.Core.Services.Ads;
using CommunityHub.Application.Services.Dtos.Ads;
using CommunityHub.Ui.Helpers.Alerts;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.Views.Resident.Buildings.Ads.Create;
using System.Globalization;

namespace CommunityHub.Ui.ViewModels.Resident.Buildings.Ads.Create
{
    public class SubmissionResult
    {
        public PublishmentValidationResult ValidationResult { get; }

        private readonly AdDto? _ad;
        public AdDto PublishedAd => _ad ?? throw new InvalidOperationException("Submission wasn't successful.");

        public SubmissionResult(PublishmentValidationResult validationResult, AdDto? ad = null)
        {
            ValidationResult = validationResult;
            _ad = ad;
        }

        public bool IsSuccessful => ValidationResult is PublishmentValidationResult.Valid && _ad is not null;
    }

    public class CreateAdViewModel: BaseViewModel
    {
        private readonly CultureInfo _culture = new("sr-Latn-RS");
        private readonly DateTime _today = DateTime.Today;

        private readonly AdService _adService;

        private readonly long _userId;
        private readonly long _buildingId;

        public CreateAdViewModel(
            long userId,
            long buildingId,
            IMainNavigationConsumer navigationConsumer
        ) : base(navigationConsumer)
        {
            _adService = new(
                Injector.CreateInstance<IAdRepository>(),
                Injector.CreateInstance<IAdTimeSlotRepository>()
            );

            _userId = userId;
            _buildingId = buildingId;

            PublishCommand = new RelayCommand(Publish, () => IsFormValid);
            OpenSelectTypePageCommand = new RelayCommand(OpenSelectTypePage);
            OpenSelectCategoryPageCommand = new RelayCommand(OpenSelectCategoryPage);
        }

        public RelayCommand PublishCommand { get; }
        public RelayCommand OpenSelectTypePageCommand { get; }
        public RelayCommand OpenSelectCategoryPageCommand { get; }

        private void Publish()
        {
            var result = Submit();

            if (result.IsSuccessful)
            {
                ShowSuccessAlert(result.PublishedAd);
                return;
            }

            ShowPublishmentValidationResult(result.ValidationResult);
        }

        private void OpenSelectTypePage()
        {
            Push(
                new SelectAdTypePage(selectedType: SelectedType),
                onPop: (object? value) => {
                    if (value is not AdType type) return;
                    SelectedType = type;
                }
            );
        }

        private void OpenSelectCategoryPage()
        {
            Push(
                new SelectAdCategoryPage(selectedCategory: SelectedCategory),
                onPop: (object? value) => {
                    if (value is not AdCategory category) return;
                    SelectedCategory = category;
                }
            );
        }
        
        private SubmissionResult Submit()
        {
            if (!IsFormValid)
                throw new InvalidOperationException("Invalid form can't be submitted.");

            var ad = new AdDto(
                authorId: _userId,
                buildingId: _buildingId,
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
                _adService.NotifyAuthorsOfSubscribedMatchingAds(publishedAd);
                return new(validationResult, publishedAd);
            }

            return new(validationResult);
        }

        private void ShowPublishmentValidationResult(PublishmentValidationResult result)
        {
            switch (result)
            {
                case PublishmentValidationResult.DuplicateAdExists:
                    ShowAlert(
                        Alert.Create(
                            "Greška",
                            "Objava oglasa nije bila moguća. Već imate aktivan oglas u ovoj zgradi sa istim tipom, kategorijom i preklapajućim datumima. Korigujte opseg datuma i pokušajte ponovo.",
                            AlertType.Error
                        )
                        .AddOption("U redu")
                    );
                    break;
            }
        }

        private void ShowSuccessAlert(AdDto publishedAd)
            => ShowAlert(
                Alert.Create(
                    "Oglas objavljen",
                    "Vaš oglas je uspješno objavljen i biće prikazan ostalim stanarima na oglasnoj tabli zgrade.",
                    AlertType.Info
                )
                .AddOption(
                    "Nastavi",
                    () => Pop(publishedAd)
                )
            );

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

                PublishCommand.RaiseCanExecuteChanged();
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

                PublishCommand.RaiseCanExecuteChanged();
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

                PublishCommand.RaiseCanExecuteChanged();
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

                PublishCommand.RaiseCanExecuteChanged();
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

                PublishCommand.RaiseCanExecuteChanged();
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
    }
}
