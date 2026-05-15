using System.Windows;
using CommunityHub.Ui.ViewModels.Citizen.Events;

namespace CommunityHub.Ui.Views.Citizen.Events
{
    public partial class ApplyToVolunteerWindow : Window
    {
        private readonly ApplyToVolunteerViewModel _viewModel;

        public ApplyToVolunteerWindow(long userId, long eventId)
        {
            InitializeComponent();
            _viewModel = new ApplyToVolunteerViewModel(userId, eventId);
            DataContext = _viewModel;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            var validationError = _viewModel.ValidateRegistration();
            if (validationError != null)
            {
                MessageBox.Show(validationError, "Obaveštenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _viewModel.Register();
            this.DialogResult = true;
        }
    }
}

