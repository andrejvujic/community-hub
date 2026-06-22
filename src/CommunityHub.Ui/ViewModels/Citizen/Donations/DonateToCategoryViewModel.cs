using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityHub.Application.Services.Dtos.Neighborhoods.NeighborhoodBudgets;
using CommunityHub.Application.Services.Dtos.Users;
using CommunityHub.Ui.Helpers.Commands;

namespace CommunityHub.Ui.ViewModels.Citizen.Donations
{
    public class DonateToCategoryViewModel : INotifyPropertyChanged
    {
        private BudgetCategoryBalanceDto _category;
        private CitizenDto _citizen;
        private decimal _donationAmount;

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler? CloseRequested;
        public event EventHandler? ConfirmRequested;

        public string CategoryName => _category?.Category.Name ?? "";
        public string CategoryBudgetDisplay => $"{_category?.TotalAmount:N2} RSD";
        public string CitizenName => $"{_citizen?.Name} {_citizen?.Surname}";
        public string CurrentDate => DateTime.Today.ToString("d.M.yyyy.");

        public decimal DonationAmount
        {
            get => _donationAmount;
            set
            {
                if (_donationAmount != value)
                {
                    _donationAmount = value;
                    OnPropertyChanged(nameof(DonationAmount));
                    OnPropertyChanged(nameof(IsValid));
                    ((RelayCommand<object>)ConfirmCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public string DonationAmountText
        {
            get => _donationAmount > 0 ? _donationAmount.ToString() : "";
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    DonationAmount = 0;
                }
                else if (decimal.TryParse(value, out decimal newAmount) && newAmount > 0)
                {
                    DonationAmount = newAmount;
                }
            }
        }

        public bool IsValid => _donationAmount > 0;

        public ICommand CancelCommand { get; }
        public ICommand ConfirmCommand { get; }

        public DonateToCategoryViewModel(BudgetCategoryBalanceDto category, CitizenDto citizen)
        {
            _category = category;
            _citizen = citizen;
            _donationAmount = 0;

            CancelCommand = new RelayCommand<object>(_ => CloseRequested?.Invoke(this, EventArgs.Empty));
            ConfirmCommand = new RelayCommand<object>(_ => ConfirmRequested?.Invoke(this, EventArgs.Empty), _ => IsValid);
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
