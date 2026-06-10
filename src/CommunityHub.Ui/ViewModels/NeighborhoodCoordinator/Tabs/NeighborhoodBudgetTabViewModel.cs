using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using CommunityHub.Application.Services.Dtos.Neighborhoods;
using CommunityHub.Application.Services.Dtos.Neighborhoods.NeighborhoodBudgets;
using CommunityHub.Application.Services.Neighborhoods;
using CommunityHub.Application.Services.Neighborhoods.NeighborhoodBudgets;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Views.NeighborhoodCoordinator.Windows;

namespace CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Tabs
{
    public class NeighborhoodBudgetViewModel : INotifyPropertyChanged
    {
        private readonly NeighborhoodService _neighborhoodService = new();
        private readonly BudgetCategoryBalanceService _balanceService = new();
        private readonly BudgetExpenseService _expenseService = new();
        private readonly BudgetDonationService _donationService = new();
        private long _coordinatorId;

        public ObservableCollection<NeighborhoodDto> Neighborhoods { get; } = new();
        public ObservableCollection<BudgetCategoryBalanceDto> BudgetCategories { get; } = new();
        public ObservableCollection<BudgetExpenseDto> Expenses { get; } = new();
        public ObservableCollection<BudgetDonationDto> Donations { get; } = new();

        private NeighborhoodDto? _selectedNeighborhood;
        public NeighborhoodDto? SelectedNeighborhood
        {
            get => _selectedNeighborhood;
            set
            {
                if (_selectedNeighborhood == value) return;
                _selectedNeighborhood = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(BudgetPanelVisibility));
                if (_selectedNeighborhood != null)
                {
                    BudgetTitleText = $"Detalji budžeta kvarta {_selectedNeighborhood.Name}";
                    LoadBudgetData();
                }
                SelectedCategoryBalance = null;
            }
        }

        private BudgetCategoryBalanceDto? _selectedCategoryBalance;
        public BudgetCategoryBalanceDto? SelectedCategoryBalance
        {
            get => _selectedCategoryBalance;
            set
            {
                if (_selectedCategoryBalance == value) return;
                _selectedCategoryBalance = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CategoryDetailsPanelVisibility));
                if (_selectedCategoryBalance != null)
                {
                    DonationsTitleText = $"Donacije: {_selectedCategoryBalance.Category.Name}";
                    ExpensesTitleText = $"Troškovi: {_selectedCategoryBalance.Category.Name}";
                    LoadExpensesForSelectedCategory();
                    LoadDonationsForSelectedCategory();
                }
            }
        }

        private string _budgetTitleText = "Detalji budžeta kvarta";
        public string BudgetTitleText
        {
            get => _budgetTitleText;
            set
            {
                if (_budgetTitleText == value) return;
                _budgetTitleText = value;
                OnPropertyChanged();
            }
        }

        private string _donationsTitleText = "Donacije";
        public string DonationsTitleText
        {
            get => _donationsTitleText;
            set
            {
                if (_donationsTitleText == value) return;
                _donationsTitleText = value;
                OnPropertyChanged();
            }
        }

        private string _expensesTitleText = "Troškovi";
        public string ExpensesTitleText
        {
            get => _expensesTitleText;
            set
            {
                if (_expensesTitleText == value) return;
                _expensesTitleText = value;
                OnPropertyChanged();
            }
        }

        private string _descriptionInput = string.Empty;
        public string DescriptionInput
        {
            get => _descriptionInput;
            set
            {
                if (_descriptionInput == value) return;
                _descriptionInput = value;
                OnPropertyChanged();
            }
        }

        private string _amountInput = string.Empty;
        public string AmountInput
        {
            get => _amountInput;
            set
            {
                if (_amountInput == value) return;
                _amountInput = value;
                OnPropertyChanged();
            }
        }

        public Visibility BudgetPanelVisibility => SelectedNeighborhood != null ? Visibility.Visible : Visibility.Collapsed;
        public Visibility CategoryDetailsPanelVisibility => SelectedCategoryBalance != null ? Visibility.Visible : Visibility.Collapsed;

        public ICommand RegisterExpenseCommand { get; }

        public NeighborhoodBudgetViewModel()
        {
            RegisterExpenseCommand = new RelayCommand(ExecuteRegisterExpense);
        }

        public void Initialize(long coordinatorId)
        {
            _coordinatorId = coordinatorId;
            LoadNeighborhoods();
        }

        public void LoadNeighborhoods()
        {
            Neighborhoods.Clear();
            var neighborhoods = _neighborhoodService.GetByCoordinator(_coordinatorId);
            foreach (var n in neighborhoods)
                Neighborhoods.Add(n);
        }

        private void LoadBudgetData()
        {
            if (SelectedNeighborhood == null) return;

            BudgetCategories.Clear();
            var allExpenses = _expenseService.GetByNeighborhood(SelectedNeighborhood.Id);
            var allDonations = _donationService.GetDonationsForNeighborhood(SelectedNeighborhood.Id);
            var balances = _balanceService.GetByNeighborhood(SelectedNeighborhood.Id, allExpenses, allDonations);
            foreach (var b in balances)
            {
                BudgetCategories.Add(b);
            }
        }

        private void LoadExpensesForSelectedCategory()
        {
            Expenses.Clear();
            if (SelectedNeighborhood == null || SelectedCategoryBalance == null) return;

            var allExpenses = _expenseService.GetByNeighborhood(SelectedNeighborhood.Id);
            var categoryExpenses = allExpenses.Where(ex => ex.BudgetCategoryBalance.Id == SelectedCategoryBalance.Id);

            foreach (var ex in categoryExpenses)
                Expenses.Add(ex);
        }

        private void LoadDonationsForSelectedCategory()
        {
            Donations.Clear();
            if (SelectedNeighborhood == null || SelectedCategoryBalance == null) return;

            var categoryDonations = _donationService.GetDonationsForCategory(
                SelectedNeighborhood.Id, 
                SelectedCategoryBalance.Category.Id);

            foreach (var d in categoryDonations)
                Donations.Add(d);
        }

        private void ExecuteRegisterExpense()
        {
            var window = System.Windows.Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive)
                ?? System.Windows.Application.Current.MainWindow;
            if (!IsValidInput(window, out decimal amount, out string description)) return;

            try
            {
                SaveExpenseAndRefresh(window, amount, description);
            }
            catch (InvalidOperationException ex)
            {
                new InfoDialog("Nedovoljno sredstava", ex.Message, window).ShowDialog();
            }
        }

        private bool IsValidInput(Window win, out decimal amount, out string desc)
        {
            amount = 0;
            desc = (DescriptionInput ?? "").Trim();

            if (SelectedCategoryBalance is null)
                return ShowError(win, "Odaberite kategoriju", "Greška");

            if (string.IsNullOrEmpty(desc))
                return ShowError(win, "Greška", "Unesite opis troška.");

            if (!decimal.TryParse(AmountInput?.Trim(), out amount) || amount <= 0)
                return ShowError(win, "Greška", "Unesite ispravan iznos.");

            return true;
        }

        private void SaveExpenseAndRefresh(Window window, decimal amount, string desc)
        {
            var coordinator = SelectedNeighborhood!.Coordinator;

            try
            {
                _expenseService.Create(SelectedCategoryBalance!, coordinator, amount, desc);
                _balanceService.UpdateTotalAmount(SelectedCategoryBalance!, amount);
                _neighborhoodService.DeductFromBudget(SelectedNeighborhood!, amount);
                new InfoDialog("Uspeh", "Trošak je uspešno evidentiran.", window).ShowDialog();
                ResetAndRefreshForm();
            }
            catch (InvalidOperationException ex)
            {
                new InfoDialog("Nedovoljno sredstava", ex.Message, window).ShowDialog();
            }
        }

        private void ResetAndRefreshForm()
        {
            long previouslySelectedId = SelectedCategoryBalance!.Id;
            AmountInput = string.Empty;
            DescriptionInput = string.Empty;

            LoadBudgetData();
            SelectedCategoryBalance = BudgetCategories.FirstOrDefault(b => b.Id == previouslySelectedId);
        }

        private bool ShowError(Window win, string title, string msg)
        {
            new InfoDialog(title, msg, win).ShowDialog();
            return false;
        }

        public void SelectBudgetCategory(long neighborhoodId, long budgetCategoryId)
        {
            var neighborhood = Neighborhoods.FirstOrDefault(n => n.Id == neighborhoodId);
            if (neighborhood != null)
            {
                SelectedNeighborhood = neighborhood;

                var categoryBalance = BudgetCategories.FirstOrDefault(b => b.Category.Id == budgetCategoryId);
                if (categoryBalance != null)
                {
                    SelectedCategoryBalance = categoryBalance;
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}