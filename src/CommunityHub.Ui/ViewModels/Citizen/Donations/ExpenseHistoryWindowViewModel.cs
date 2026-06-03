using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityHub.Application.Services.Neighborhoods.NeighborhoodBudgets;
using CommunityHub.Application.Services.Dtos.Neighborhoods.NeighborhoodBudgets;
using CommunityHub.Ui.Helpers.Commands;

namespace CommunityHub.Ui.ViewModels.Citizen.Donations
{
    public class ExpenseHistoryWindowViewModel : INotifyPropertyChanged
    {
        private readonly BudgetExpenseService _expenseService = new();
        private BudgetCategoryBalanceDto _category;
        private BudgetExpenseDto? _selectedExpense;

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler? CloseRequested;

        public ObservableCollection<BudgetExpenseDto> Expenses { get; } = new();

        public string CategoryName => _category?.Category.Name ?? "";
        public string CategoryBudgetDisplay => $"{_category?.TotalAmount:N2} RSD";

        public BudgetExpenseDto? SelectedExpense
        {
            get => _selectedExpense;
            set
            {
                if (_selectedExpense != value)
                {
                    _selectedExpense = value;
                    OnPropertyChanged(nameof(SelectedExpense));
                }
            }
        }

        public ICommand CloseCommand { get; }

        public ExpenseHistoryWindowViewModel(BudgetCategoryBalanceDto category)
        {
            _category = category;
            CloseCommand = new RelayCommand<object>(_ => CloseRequested?.Invoke(this, EventArgs.Empty));
            LoadExpenses();
        }

        private void LoadExpenses()
        {
            var expenses = _expenseService.GetByCategory(_category.Category.Id);
            foreach (var expense in expenses)
            {
                Expenses.Add(expense);
            }
        }

        public void SelectFirstExpense()
        {
            if (Expenses.Count > 0)
            {
                SelectedExpense = Expenses[0];
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
