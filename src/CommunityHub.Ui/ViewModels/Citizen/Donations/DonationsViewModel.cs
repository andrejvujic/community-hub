using CommunityHub.Application.Services.Dtos.Neighborhoods.NeighborhoodBudgets;
using CommunityHub.Application.Services.Dtos.Users;
using CommunityHub.Application.Services.Neighborhoods.NeighborhoodBudgets;
using CommunityHub.Application.Services.Users;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.ViewModels.Citizen.Help;
using CommunityHub.Ui.Views.Citizen.Help;
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
        private readonly BudgetDonationService _donationService = new();

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
        public ICommand ShowHelpCommand { get; }

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

        public CitizenDto? Citizen => _citizen;

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
            ShowHelpCommand = new RelayCommand<object>(_ => ShowHelp());

            LoadData();
        }

        private void ShowHelp()
        {
            var helpWindow = new QuickHelpWindow(
                "Brza pomoć - Donacije",
                "Pregledajte budžete po kategorijama u vašem kvartu. Možete donirati sredstva za odabrane kategorije i pregledati istoriju troškova.",
                new List<KeyboardShortcut>
                {
                    new() { Keys = "F1-F7", Description = "Brza navigacija kroz kartice" },
                    new() { Keys = "Ctrl+T", Description = "Uključi/isključi prikaz objašnjenja" },
                    new() { Keys = "Ctrl+1", Description = "Prikaz istorije troškova kategorije" },
                    new() { Keys = "Ctrl+2", Description = "Ostavi donaciju za kategoriju" }
                }
            );
            helpWindow.ShowDialog();
        }

        private void LoadData()
        {
            _citizen = _citizenService.GetCitizenById(_userId);

            if (_citizen?.Membership != null)
            {
                RefreshData();
                Membership = _citizen.Membership;
            }
            else
            {
                Membership = null;
            }
        }

        public void RefreshData()
        {
            var selectedCategoryId = SelectedCategory?.Category.Id;
            _categories.Clear();

            var neighborhoodId = _citizen!.Membership!.Neighborhood.Id;

            PopulateBudgetData(neighborhoodId);

            SelectedCategory = Categories.FirstOrDefault(c => c.Category.Id == selectedCategoryId) ?? Categories.FirstOrDefault();
        }

        public void PopulateBudgetData(long neighborhoodId)
        {
            var balanceDtos = _balanceService.GetByNeighborhood(neighborhoodId);
            foreach (var balanceDto in balanceDtos) _categories.Add(balanceDto);

            var updatedCitizen = _citizenService.GetCitizenById(_citizen!.Id);
            if (updatedCitizen?.Membership?.Neighborhood != null)
            {
                NeighborhoodBudgetTotal = updatedCitizen.Membership.Neighborhood.Budget;
            }
        }

        public void CreateDonation(BudgetCategoryBalanceDto category, decimal amount)
        {
            if (_citizen == null || category.Neighborhood == null) return;

            var donationDto = new BudgetDonationDto(0, category.Neighborhood, category.Category, _citizen, amount, DateOnly.FromDateTime(DateTime.Today));

            _donationService.Create(donationDto);
            RefreshData();
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

