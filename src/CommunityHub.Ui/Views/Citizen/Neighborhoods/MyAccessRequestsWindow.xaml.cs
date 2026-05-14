using System.Windows;
using System.Windows.Input;
using CommunityHub.Application.Helpers.Sorting;
using CommunityHub.Application.Services.Dtos.Neighborhoods;
using CommunityHub.Ui.ViewModels.Citizen.Neighborhoods;

namespace CommunityHub.Ui.Views.Citizen.Neighborhoods
{
    public partial class MyAccessRequestsWindow : Window
    {
        private MyAccessRequestsWindowViewModel? _viewModel;

        public MyAccessRequestsWindow(long citizenId)
        {
            InitializeComponent();
            _viewModel = new MyAccessRequestsWindowViewModel(citizenId);
            DataContext = _viewModel;
            KeyDown += MyAccessRequestsWindow_KeyDown;
        }

        private void DeleteRequest_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.DataContext is NeighborhoodAccessRequestDto request)
            {
                var result = MessageBox.Show("Da li ste sigurni da želite da obrišete ovaj zahtev?", "Potvrda brisanja", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result != MessageBoxResult.Yes) return;

                _viewModel?.DeleteRequest(request);
            }
        }

        private void Toggle_Changed(object sender, RoutedEventArgs e)
        {
            if (_viewModel == null) return;

            var order = NewestToggle?.IsChecked == true ? RequestOrder.CreationDateDescending : RequestOrder.CreationDateAscending;
            _viewModel.SelectedOrder = order;

            var filterByStatus = GroupByStatusToggle?.IsChecked == true;
            _viewModel.FilterByStatus = filterByStatus;
        }

        private void MyAccessRequestsWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.Control) return;

            if (e.Key == Key.D1 || e.Key == Key.NumPad1)
            {
                if (NewestToggle is { } && OldestToggle is { })
                {
                    if (NewestToggle.IsChecked == true)
                        OldestToggle.IsChecked = true;
                    else
                        NewestToggle.IsChecked = true;
                }

                e.Handled = true;
                return;
            }

            if (e.Key == Key.D2 || e.Key == Key.NumPad2)
            {
                if (GroupByStatusToggle is { })
                    GroupByStatusToggle.IsChecked = !(GroupByStatusToggle.IsChecked == true);

                e.Handled = true;
                return;
            }
        }
    }
}
