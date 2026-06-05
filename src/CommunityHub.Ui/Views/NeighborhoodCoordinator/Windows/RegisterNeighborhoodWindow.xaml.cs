using System.Windows;
using System.Windows.Input;
using CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Windows;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Windows
{
    public partial class RegisterNeighborhoodWindow : Window
    {
        private readonly RegisterNeighborhoodWindowViewModel _viewModel;

        public RegisterNeighborhoodWindow(long coordinatorId)
        {
            InitializeComponent();

            _viewModel = new RegisterNeighborhoodWindowViewModel(coordinatorId);
            DataContext = _viewModel;
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
    }
}