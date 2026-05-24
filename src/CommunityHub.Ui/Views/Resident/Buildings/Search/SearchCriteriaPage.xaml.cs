using CommunityHub.Application.Helpers.Search;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident.Buildings.Search;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.Search
{
    public partial class SearchCriteriaPage : MainNavigationConsumerPage
    {
        private readonly SearchCriteriaViewModel _viewModel;

        public SearchCriteriaPage(
            Action<BuildingSearchParameters> setSearchParameters,
            BuildingSearchParameters? initialSearchParameters = null
        )
        {
            InitializeComponent();
            _viewModel = new SearchCriteriaViewModel(
                setSearchParameters: setSearchParameters,
                page: this,
                initialSearchParameters: initialSearchParameters
            );
            DataContext = _viewModel;
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e) => Pop();

    }
}
