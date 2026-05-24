using CommunityHub.Application.Domain.Buildings;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident.Buildings.Search;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.Search
{

    public partial class SearchBuildingsPage : MainNavigationConsumerPage
    {
        private readonly SearchBuildingsViewModel _viewModel;
        public SearchBuildingsPage(long userId)
        {
            InitializeComponent();
            _viewModel = new SearchBuildingsViewModel(userId, this, () => BuildingsScrollViewer.ScrollToTop());
            DataContext = _viewModel;
            _viewModel.UpdateSearchResults();
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e) => Pop();

    }
}
