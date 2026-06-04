using System.Windows;
using System.Windows.Input;
using CommunityHub.Ui.ViewModels.Citizen.Neighborhoods;

namespace CommunityHub.Ui.Views.Citizen.Neighborhoods
{
    public partial class MyAccessRequestsWindow : Window
    {
        private MyAccessRequestsViewModel? _viewModel;

        public MyAccessRequestsWindow(long citizenId)
        {
            InitializeComponent();
            _viewModel = new MyAccessRequestsViewModel(citizenId);
            _viewModel.CloseRequested += (_, _) => Close();
            _viewModel.DeleteRequested += (_, args) =>
            {
                var result = MessageBox.Show("Da li ste sigurni da želite da obrišete ovaj zahtev?", "Potvrda brisanja", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result != MessageBoxResult.Yes) return;

                _viewModel.DeleteRequest(args.Request);
                _viewModel.SelectedRequest = null;
            };
            DataContext = _viewModel;
        }

        private void Header_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}