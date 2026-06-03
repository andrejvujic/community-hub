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
                DataContext = _viewModel;
            };
        }
    }
}
