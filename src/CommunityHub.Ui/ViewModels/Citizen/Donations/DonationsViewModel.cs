using CommunityHub.Application.Services;
using CommunityHub.Application.Services.Users;
using CommunityHub.Application.Services.Dtos.Users;
using CommunityHub.Application.Services.Dtos.Neighborhoods.NeighborhoodBudgets;
using CommunityHub.Application.Services.Neighborhoods.NeighborhoodBudgets;
using CommunityHub.Ui.Helpers.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace CommunityHub.Ui.ViewModels.Citizen.Donations
{
    public class DonationsViewModel : INotifyPropertyChanged
    {
        private readonly long _userId;
        private readonly BudgetCategoryBalanceService _balanceService = new();
        private readonly CitizenService _citizenService = new();

        private CitizenDto? _citizen;
        private ObservableCollection<BudgetCategoryBalanceDto> _categories = new();
        private object? _membership;
        private BudgetCategoryBalanceDto? _selectedCategory;
        private decimal _neighborhoodBudgetTotal;

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler? ViewExpenseHistoryRequested;
        public event EventHandler? MakeDonationRequested;

        public ICommand ViewExpenseHistoryCommand { get; }
        public ICommand MakeDonationCommand { get; }

        public ObservableCollection<BudgetCategoryBalanceDto> Categories => _categories;

        public BudgetCategoryBalanceDto? SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (_selectedCategory == value) return;
                _selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
            }
        }

        public object? Membership
        {
            get => _membership;
            private set
            {
                if (_membership == value) return;
                _membership = value;
                OnPropertyChanged(nameof(Membership));
            }
        }

        public decimal NeighborhoodBudgetTotal
        {
            get => _neighborhoodBudgetTotal;
            private set
            {
                if (_neighborhoodBudgetTotal == value) return;
                _neighborhoodBudgetTotal = value;
                OnPropertyChanged(nameof(NeighborhoodBudgetTotal));
            }
        }

        public DonationsViewModel(long userId)
        {
            _userId = userId;

            ViewExpenseHistoryCommand = new RelayCommand<object>(_ => ViewExpenseHistoryRequested?.Invoke(this, EventArgs.Empty));
            MakeDonationCommand = new RelayCommand<object>(_ => MakeDonationRequested?.Invoke(this, EventArgs.Empty));

            LoadData();
        }

        private void LoadData()
        {
            _citizen = _citizenService.GetCitizenById(_userId);

            if (_citizen?.Membership != null)
            {
                RefreshCategories();
                Membership = _citizen.Membership;
            }
            else
            {
                Membership = null;
            }
        }

        public void RefreshCategories()
        {
            _categories.Clear();

            if (_citizen?.Membership == null) return;

            var neighborhoodId = _citizen.Membership.Neighborhood.Id;
            var balanceDtos = _balanceService.GetByNeighborhood(neighborhoodId);

            foreach (var balanceDto in balanceDtos)
            {
                _categories.Add(balanceDto);
            }

            if (_categories.Count > 0)
            {
                SelectedCategory = _categories[0];
            }

            NeighborhoodBudgetTotal = _citizen.Membership.Neighborhood.Budget;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

