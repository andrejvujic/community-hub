using CommunityHub.Application.Domain.Ads;
using CommunityHub.Application.Modules.Buildings.Core.Services.Ads;
using CommunityHub.Ui.Helpers.Alerts;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident.Buildings.Ads.Create;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.Ads.Create
{
    public partial class CreateAdPage : MainNavigatorConsumerPage, IAlertConsumer
    {
        private readonly CreateAdViewModel _viewModel;

        public CreateAdPage(long userId, long buildingId)
        {
            InitializeComponent();
            _viewModel = new(userId, buildingId);
            DataContext = _viewModel;
        }

        private void PublishAdButton_Click(object sender, RoutedEventArgs e)
        {
            var result = _viewModel.Submit();

            if (result.IsSuccessful)
            {
                ShowSuccessAlert(result.PublishedAd);
                return;
            }

            ShowPublishmentValidationResult(result.ValidationResult);
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e) => Pop();

        private void SelectTypeButton_Click(object sender, MouseButtonEventArgs e)
        {
            Push(
                new SelectAdTypePage(selectedType: _viewModel.SelectedType),
                onPop: (object? value) => {
                    if (value is not AdType type) return;
                    _viewModel.SelectedType = type;
                }
            );
        }

        private void SelectCategoryButton_Click(object sender, MouseButtonEventArgs e)
        {
            Push(
                new SelectAdCategoryPage(selectedCategory: _viewModel.SelectedCategory),
                onPop: (object? value) => {
                    if (value is not AdCategory category) return;
                    _viewModel.SelectedCategory = category;
                }
            );
        }

        private void ShowPublishmentValidationResult(PublishmentValidationResult result)
        {
            switch (result)
            {
                case PublishmentValidationResult.DuplicateAdExists:
                    if (this is IAlertConsumer consumer)
                        consumer.ShowAlert(
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

        private void ShowSuccessAlert(Ad publishedAd)
        {
            if (this is IAlertConsumer consumer)
                consumer.ShowAlert(
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
        }

        private void ToggleOverlayVisibility(FrameworkElement overlay)
            =>
                overlay.Visibility = overlay.Visibility switch
                {
                    Visibility.Visible => Visibility.Collapsed,
                    Visibility.Collapsed => Visibility.Visible,
                    _ => Visibility.Visible
                };

        private void ToggleSelectDateRangeMenuButton_Click(object sender, EventArgs e)
            => ToggleOverlayVisibility(SelectDateRangeOverlay);

        private void ToggleStartDatePickerButton_Click(object sender, EventArgs e)
            => ToggleOverlayVisibility(StartDatePicker);

        private void ToggleEndDatePickerButton_Click(object sender, EventArgs e)
            => ToggleOverlayVisibility(EndDatePicker);
    }
}
