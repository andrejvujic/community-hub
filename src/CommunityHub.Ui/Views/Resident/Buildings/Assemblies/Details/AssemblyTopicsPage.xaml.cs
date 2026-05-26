using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident.Buildings.Assemblies;

namespace CommunityHub.Ui.Views.Resident.Buildings.Assemblies.Details
{
    public partial class AssemblyTopicsPage : MainNavigationConsumerPage
    {
        private readonly AssemblyTopicsViewModel _viewModel;

        public AssemblyTopicsPage(long residentId, long assemblyId)
        {
            InitializeComponent();

            _viewModel = new AssemblyTopicsViewModel(residentId, assemblyId, this);
            DataContext = _viewModel;

            _viewModel.UpdateTopics();
        }
    }
}
