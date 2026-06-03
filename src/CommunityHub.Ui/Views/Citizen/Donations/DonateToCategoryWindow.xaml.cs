using System.Windows;
using System.Windows.Input;
using CommunityHub.Application.Services.Dtos.Neighborhoods.NeighborhoodBudgets;
using CommunityHub.Application.Services.Dtos.Users;
using CommunityHub.Ui.ViewModels.Citizen.Donations;

namespace CommunityHub.Ui.Views.Citizen.Donations
{
    public partial class DonateToCategoryWindow : Window
    {
        private DonateToCategoryViewModel? _viewModel;
        public decimal DonationAmount => _viewModel?.DonationAmount ?? 0;

        public DonateToCategoryWindow(BudgetCategoryBalanceDto category, CitizenDto citizen)
        {
            InitializeComponent();
            _viewModel = new DonateToCategoryViewModel(category, citizen);
            DataContext = _viewModel;

            if (_viewModel != null)
            {
                _viewModel.CloseRequested += (_, _) => DialogResult = false;
                _viewModel.ConfirmRequested += (_, _) => DialogResult = true;
            }

            Loaded += (_, _) => DonationAmountTextBox.Focus();
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
