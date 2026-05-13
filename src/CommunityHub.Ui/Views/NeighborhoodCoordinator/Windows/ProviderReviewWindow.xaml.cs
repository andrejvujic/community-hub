using System.Windows;
using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;
using CommunityHub.Ui.ViewModels;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Windows
{
    public partial class ProviderReviewWindow : Window
    {
        private readonly BookedJobViewModel _bookedJobViewModel;
        private readonly long _coordinatorId;
        private readonly ProviderReviewRepository _providerReviewRepository;

        public ProviderReviewWindow(
            BookedJobViewModel bookedJobViewModel,
            long coordinatorId,
            ProviderReviewRepository providerReviewRepository)
        {
            InitializeComponent();
            _bookedJobViewModel = bookedJobViewModel;
            _coordinatorId = coordinatorId;
            _providerReviewRepository = providerReviewRepository;
            Populate();
        }

        private void Populate()
        {
            TitleTextBlock.Text = _bookedJobViewModel.Review is null
                ? $"Ocenite uslugu: {_bookedJobViewModel.ServiceName}"
                : $"Izmenite recenziju: {_bookedJobViewModel.ServiceName}";

            if (_bookedJobViewModel.Review is not null)
                PopulateExistingReview(_bookedJobViewModel.Review);
        }

        private void PopulateExistingReview(ProviderReview review)
        {
            QualityRatingComboBox.SelectedIndex = review.QualityRating - 1;
            CommunicationRatingComboBox.SelectedIndex = review.CommunicationRating - 1;
            CommentTextBox.Text = review.Comment ?? string.Empty;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (!TryGetRatings(out int quality, out int communication)) return;
            if (!TryGetComment(out string comment)) return;

            if (_bookedJobViewModel.Review is null)
                CreateReview(quality, communication, comment);
            else
                UpdateReview(quality, communication, comment);

            DialogResult = true;
            Close();
        }

        private bool TryGetRatings(out int quality, out int communication)
        {
            quality = 0;
            communication = 0;

            if (QualityRatingComboBox.SelectedIndex < 0 ||
                CommunicationRatingComboBox.SelectedIndex < 0)
            {
                MessageBox.Show(
                    "Molimo odaberite obe ocene.",
                    "Nevažeći unos",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return false;
            }

            quality = QualityRatingComboBox.SelectedIndex + 1;
            communication = CommunicationRatingComboBox.SelectedIndex + 1;
            return true;
        }

        private bool TryGetComment(out string comment)
        {
            comment = CommentTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(comment))
            {
                MessageBox.Show(
                    "Molimo unesite komentar. Komentar je obavezan deo recenzije.",
                    "Nedostaje komentar",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void CreateReview(int quality, int communication, string? comment)
        {
            var coordinator = _bookedJobViewModel.BookedJob.Neighborhood.Coordinator;
            var review = new ProviderReview(
                _bookedJobViewModel.BookedJob,
                coordinator,
                quality,
                communication,
                DateTime.Now,
                comment: comment
            );
            _providerReviewRepository.Create(review);
        }

        private void UpdateReview(int quality, int communication, string? comment)
        {
            var updated = _bookedJobViewModel.Review!.CopyWith(
                qualityRating: quality,
                communicationRating: communication,
                comment: comment
            );
            _providerReviewRepository.Update(updated);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
