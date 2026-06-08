using System.Windows;
using System.Windows.Input;
using CommunityHub.Application.Services.Dtos.Neighborhoods.NeighborhoodBudgets;
using CommunityHub.Ui.ViewModels.Citizen.Donations;

namespace CommunityHub.Ui.Views.Citizen.Donations
{
    public partial class ExpenseHistoryWindow : Window
    {
        private ExpenseHistoryViewModel? _viewModel;

        public ExpenseHistoryWindow(BudgetCategoryBalanceDto category)
        {
            InitializeComponent();
            _viewModel = new ExpenseHistoryViewModel(category);
            DataContext = _viewModel;

            if (_viewModel != null)
            {
                _viewModel.CloseRequested += (_, _) => Close();
            }
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
