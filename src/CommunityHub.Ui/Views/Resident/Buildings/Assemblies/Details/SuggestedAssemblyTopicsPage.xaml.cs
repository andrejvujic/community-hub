using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident.Buildings.Assemblies;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.Assemblies.Details
{
    public partial class SuggestedAssemblyTopicsPage : MainNavigationConsumerPage
    {
        private readonly SuggestedAssemblyTopicsViewModel _viewModel;

        public SuggestedAssemblyTopicsPage(long residentId, long assemblyId)
        {
            InitializeComponent();

            _viewModel = new SuggestedAssemblyTopicsViewModel(residentId, assemblyId, this);
            DataContext = _viewModel;

            _viewModel.UpdateTopics();
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e)
            => Pop();
    }
}
