using System.Windows;
using System.Windows.Controls;
using CommunityHub.Ui.ViewModels.Citizen.Donations;

namespace CommunityHub.Ui.Views.Citizen.Donations
{
    public partial class DonationsPage : Page
    {
        private DonationsViewModel? _viewModel;

        public DonationsPage(long userId)
        {
            InitializeComponent();

            Loaded += (_, _) =>
            {
                _viewModel = new DonationsViewModel(userId);
                _viewModel.ViewExpenseHistoryRequested += (_, _) => HandleViewExpenseHistory();
                _viewModel.MakeDonationRequested += (_, _) => HandleMakeDonation();
                DataContext = _viewModel;
            };
        }

        private void HandleViewExpenseHistory()
        {
            if (_viewModel?.SelectedCategory != null)
            {
                var wnd = new ExpenseHistoryWindow(_viewModel.SelectedCategory);
                wnd.Owner = Window.GetWindow(this);
                wnd.ShowDialog();
            }
        }

        private void HandleMakeDonation()
        {
            if (_viewModel?.SelectedCategory != null && _viewModel.Citizen != null)
            {
                var wnd = new DonateToCategoryWindow(_viewModel.SelectedCategory, _viewModel.Citizen);
                wnd.Owner = Window.GetWindow(this);
                if (wnd.ShowDialog() == true)
                {
                    _viewModel.CreateDonation(_viewModel.SelectedCategory, wnd.DonationAmount);
                }
            }
        }
    }
}
