using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CommunityHub.Application.Domain.Addresses;
using CommunityHub.Application.Services.Dtos.Addresses;
using CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Windows;
using Microsoft.Win32;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Windows
{
    public partial class RegisterNeighborhoodWindow : Window
    {
        private readonly RegisterNeighborhoodWindowViewModel _viewModel;

        public RegisterNeighborhoodWindow(long coordinatorId)
        {
            InitializeComponent();

            _viewModel = new RegisterNeighborhoodWindowViewModel(coordinatorId);

            if (_viewModel.Coordinator == null)
            {
                new InfoDialog("Greška", "Korisnik nije pronađen.", this).ShowDialog();
                Close();
                return;
            }

            DataContext = _viewModel;
        }


        private void AddStreetSection_Click(object sender, RoutedEventArgs e)
        {
            var side = (SideComboBox.SelectedItem as ComboBoxItem)?.Tag as ParityType? ?? ParityType.Both;

            var error = _viewModel.TryAddStreetSection(
                StartNumberTextBox.Text,
                EndNumberTextBox.Text,
                side);

            if (error != null)
            {
                new InfoDialog("Greška", error, this).ShowDialog();
                return;
            }

            StartNumberTextBox.Clear();
            EndNumberTextBox.Clear();
        }

        private void NumberTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(char.IsDigit);
        }

        private void NumberTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.V &&
                (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
                e.Handled = true;
        }

        private void RemoveStreetSection_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is StreetSectionDto section)
                _viewModel.RemoveStreetSection(section);
        }

        private void SelectImages_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Images|*.jpg;*.jpeg;*.png",
                Title = "Izaberite slike kvarta"
            };

            if (dialog.ShowDialog() == true)
                _viewModel.AddImages(dialog.FileNames);
        }

        private void RemoveImage_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is string path)
                _viewModel.RemoveImage(path);
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var error = _viewModel.TryRegister();

            if (!string.IsNullOrEmpty(error))
            {
                new InfoDialog("Validacija", "Molimo popunite sledeće nedostatke:\n\n" + error, this).ShowDialog();
                return;
            }

            new InfoDialog("Uspeh", $"Kvart '{_viewModel.Name}' je uspešno registrovan!", this).ShowDialog();
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
