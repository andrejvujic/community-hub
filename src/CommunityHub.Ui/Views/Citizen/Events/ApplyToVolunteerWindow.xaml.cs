using System.Windows;
using System.Windows.Input;
using CommunityHub.Ui.ViewModels.Citizen.Events;

namespace CommunityHub.Ui.Views.Citizen.Events
{
    public partial class ApplyToVolunteerWindow : Window
    {
        private ApplyToVolunteerViewModel? _viewModel;

        public ApplyToVolunteerWindow(long userId, long eventId)
        {
            InitializeComponent();
            _viewModel = new ApplyToVolunteerViewModel(userId, eventId);
            DataContext = _viewModel;

            if (_viewModel != null)
            {
                _viewModel.ConfirmRequested += (_, _) => { DialogResult = true; };
                _viewModel.CancelRequested += (_, _) => { DialogResult = false; };
            }

            Loaded += (_, _) => { ItemsListBox.Focus(); };
        }

        private void Header_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
