using CommunityHub.Application.Helpers.Search;
using CommunityHub.Application.Services.Dtos.Neighborhoods;
using CommunityHub.Ui.ViewModels.Citizen.Neighborhoods;
using System.Windows;
using System.Windows.Controls;

namespace CommunityHub.Ui.Views.Citizen.Neighborhoods
{
    public partial class NeighborhoodsPage : Page
    {
        private NeighborhoodsViewModel? _viewModel;
        private long _userId;

        public NeighborhoodsPage(long userId)
        {
            InitializeComponent();
            _userId = userId;
            _viewModel = new NeighborhoodsViewModel(userId);
            this.DataContext = _viewModel;
        }

        private void SearchButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var window = new SearchCriteriaWindow(_viewModel?.LastSearchParameters);
            if (window.ShowDialog() == true)
            {
                _viewModel?.PerformSearch(window.Result);
            }
        }

        private void RequestAccessButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = NeighborhoodsGrid.SelectedItem as NeighborhoodDto;
            if (selected == null)
            {
                MessageBox.Show("Izaberite kvart pre slanja zahteva.", "Obaveštenje", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var dlg = new RequestNeighborhoodAccessWindow(selected.Name ?? string.Empty);
            dlg.Owner = Window.GetWindow(this);
            if (!(dlg.ShowDialog() == true && dlg.Confirmed)) return;

            if (_viewModel?.RequestNeighborhoodAccess(selected) == true)
            {
                if (_viewModel.CurrentUser?.Membership != null)
                {
                    MessageBox.Show("Vaša adresa pripada ovom kvartu. Bićete automatski pridruženi kvartu.", "Informacija", MessageBoxButton.OK, MessageBoxImage.Information);
                    MessageBox.Show("Uspešno ste pridruženi kvartu.", "Informacija", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Vaša adresa ne pripada ovom kvartu. Biće kreiran zahtev za pristup.", "Informacija", MessageBoxButton.OK, MessageBoxImage.Information);
                    MessageBox.Show("Zahtev za pristup kvartu je uspešno kreiran.", "Informacija", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else if (!string.IsNullOrEmpty(_viewModel?.ErrorMessage))
            {
                MessageBox.Show(_viewModel.ErrorMessage, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MyAccessRequestsButton_Click(object sender, RoutedEventArgs e)
        {
            var wnd = new MyAccessRequestsWindow(_userId);
            wnd.Owner = Window.GetWindow(this);
            wnd.ShowDialog();
        }
    }
}
