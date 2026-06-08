using System.Windows;
using System.Windows.Input;
using CommunityHub.Ui.ViewModels.Citizen.Events;

namespace CommunityHub.Ui.Views.Citizen.Events
{
    public partial class OrganizeEventWindow : Window
    {
        private readonly OrganizeEventViewModel _viewModel;

        public OrganizeEventWindow(long userId)
        {
            InitializeComponent();
            _viewModel = new OrganizeEventViewModel(userId);
            this.DataContext = _viewModel;
            DateTextBox.TextChanged += (s, e) => _viewModel.ParseAndSetDate(DateTextBox.Text);
            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(OrganizeEventViewModel.SelectedDate))
                {
                    if (_viewModel.SelectedDate.HasValue)
                    {
                        DateTextBox.Text = _viewModel.SelectedDate.Value.ToString("d.M.yyyy.");
                    }
                }
            };

            if (_viewModel != null)
            {
                _viewModel.CloseRequested += (_, _) => Close();
                _viewModel.ConfirmRequested += (_, _) => DialogResult = true;
                _viewModel.ToggleCalendarRequested += (_, _) => DatePickerPopup.IsOpen = !DatePickerPopup.IsOpen;
            }
        }

        private void Header_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void EventCalendar_SelectedDatesChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            DatePickerPopup.IsOpen = false;
        }
    }
}

