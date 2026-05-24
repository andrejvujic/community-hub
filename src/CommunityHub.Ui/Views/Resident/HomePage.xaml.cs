using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident;

namespace CommunityHub.Ui.Views.Resident
{
    public partial class HomePage : MainNavigationConsumerPage
    {
        private readonly HomeViewModel _viewModel;

        public HomePage(long userId)
        {
            InitializeComponent();
            
            _viewModel = new HomeViewModel(userId, this, page => LocalFrame.Navigate(page));
            DataContext = _viewModel;
            _viewModel.Initialize();
        }
    }
}
