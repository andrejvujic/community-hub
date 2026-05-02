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
using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Database.Repositories.NeighborhoodBudget;
using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.NeighborhoodBudget;
using CommunityHub.Application.Services;
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

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
        }

    }
}
