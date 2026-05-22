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
        }

        private void Header_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.AddItem();
        }

        private void ItemInputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                _viewModel.AddItem();
                e.Handled = true;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Submit();
            this.DialogResult = true;
            this.Close();
        }

        private void DiscardButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DatePickerButton_Click(object sender, RoutedEventArgs e)
        {
            DatePickerPopup.IsOpen = true;
        }

        private void EventCalendar_SelectedDatesChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            DatePickerPopup.IsOpen = false;
        }
    }
}

