using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CommunityHub.Application.Database.Repositories.NeighborhoodBudget;
using CommunityHub.Application.Database.Repositories.Neighborhoods;
using CommunityHub.Application.Domain.NeighborhoodBudget;
using CommunityHub.Application.Domain.Neighborhoods;
using CommunityHub.Application.Services;
using CommunityHub.Ui.Helpers.Converters;
using CommunityHub.Ui.ViewModels;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Pages
{
    /// <summary>
    /// Interaction logic for NeighborhoodBudgetTab.xaml
    /// </summary>
    public partial class NeighborhoodBudgetTab : UserControl
    {
        private long _coordinatorId;
        private Neighborhood? _selectedNeighborhood;
        //private List<BudgetDonation> _allDonations = new List<BudgetDonation>();
        private List<BudgetExpense> _allExpenses = new List<BudgetExpense>();

        private readonly NeighborhoodRepository _neighborhoodRepository = new NeighborhoodRepository();

        public ObservableCollection<Neighborhood> Neighborhoods { get; set; } = new ObservableCollection<Neighborhood>();
        public ObservableCollection<BudgetCategoryViewModel> BudgetCategories { get; set; } = new ObservableCollection<BudgetCategoryViewModel>();
        public ObservableCollection<BudgetExpenseViewModel> Expenses { get; set; } = new ObservableCollection<BudgetExpenseViewModel>();

        private readonly NeighborhoodBudgetService _budgetService = new(
            new BudgetCategoryBalanceRepository(),
            new BudgetExpenseRepository(),
            //new BudgetDonationRepository(),
            new NeighborhoodRepository()
        );

        public NeighborhoodBudgetTab()
        {
            InitializeComponent();
            DataContext = this;
            ExpenseDatePicker.SelectedDate = DateTime.Now;
        }

        public void Initialize(long coordinatorId)
        {
            _coordinatorId = coordinatorId;
            LoadNeighborhoods();
        }

        public void LoadNeighborhoods()
        {
            Neighborhoods.Clear();
            var neighborhoods = _neighborhoodRepository.GetByCoordinatorId(_coordinatorId);
            foreach (var neighborhood in neighborhoods)
                Neighborhoods.Add(neighborhood);
        }

        private void NeighborhoodsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedNeighborhood = NeighborhoodsListBox.SelectedItem as Neighborhood;
            if (_selectedNeighborhood is null) return;

            BudgetTitleTextBlock.Text = $"Detalji budžeta kvarta {_selectedNeighborhood.Name}";

            LoadBudgetData();
            BudgetPanel.Visibility = Visibility.Visible;
        }

        private void LoadBudgetData()
        {
            var balances = _budgetService.GetBalancesForNeighborhood(_selectedNeighborhood!.Id);
            //_allDonations = _budgetService.GetDonationsForNeighborhood(_selectedNeighborhood.Id);
            _allExpenses = _budgetService.GetExpensesForNeighborhood(_selectedNeighborhood.Id);

            BudgetCategories.Clear();
            foreach (var balance in balances)
            {
            //    var donations = _allDonations
            //        .Where(d => d.BudgetCategoryBalance.Id == balance.Id)
            //        .Sum(d => d.Amount);
                var expenses = _allExpenses
                    .Where(ex => ex.BudgetCategoryBalance.Id == balance.Id)
                    .Sum(ex => ex.Amount);

                BudgetCategories.Add(new BudgetCategoryViewModel(balance, 0, expenses));
            }
        }

        private void BudgetCategoriesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = BudgetCategoriesDataGrid.SelectedItem as BudgetCategoryViewModel;
            if (selected is null)
            {
                CategoryDetailsPanel.Visibility = Visibility.Collapsed;
                return;
            }

            PopulateCategoryDetails(selected);
            CategoryDetailsPanel.Visibility = Visibility.Visible;
        }

        private void PopulateCategoryDetails(BudgetCategoryViewModel selected)
        {
            var converter = new BudgetCategoryToStringConverter();
            var categoryName = converter.Convert(selected.Category, typeof(string), null, System.Globalization.CultureInfo.CurrentCulture);

            DonationsTitleTextBlock.Text = $"Donacije: {categoryName}";
            ExpensesTitleTextBlock.Text = $"Troškovi: {categoryName}";

            Expenses.Clear();
            var categoryExpenses = _allExpenses.Where(ex => ex.BudgetCategoryBalance.Id == selected.Balance.Id);
            foreach (var ex in categoryExpenses)
                Expenses.Add(new BudgetExpenseViewModel(ex));
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = BudgetCategoriesDataGrid.SelectedItem as BudgetCategoryViewModel;
            if (selected is null)
            {
                MessageBox.Show("Odaberite kategoriju.", "Greška",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!TryGetExpenseInputs(out decimal amount, out string description)) return;

            TryCreateExpense(selected, amount, description);
        }

        private void TryCreateExpense(
            BudgetCategoryViewModel selected,
            decimal amount,
            string description)
        {
            try
            {
                var coordinator = _selectedNeighborhood!.Coordinator;
                _budgetService.CreateExpense(selected.Balance, coordinator, amount, description);
                MessageBox.Show("Trošak je uspešno evidentiran.", "Uspeh",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                LoadBudgetData();
                RefreshCategoryDetails(selected.Balance.Id);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Nedovoljno sredstava",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private bool TryGetExpenseInputs(out decimal amount, out string description)
        {
            amount = 0;
            description = string.Empty;

            if (!decimal.TryParse(AmountTextBox.Text.Trim(), out amount) || amount <= 0)
            {
                MessageBox.Show("Unesite ispravan iznos.", "Greška",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            description = DescriptionTextBox.Text.Trim();
            if (string.IsNullOrEmpty(description))
            {
                MessageBox.Show("Unesite opis troška.", "Greška",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void RefreshCategoryDetails(long balanceId)
        {
            var refreshed = BudgetCategories.FirstOrDefault(c => c.Balance.Id == balanceId);
            if (refreshed is null) return;

            BudgetCategoriesDataGrid.SelectedItem = refreshed;
            PopulateCategoryDetails(refreshed);
        }
    }
}
