using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident.Buildings.Assemblies;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.Assemblies
{
    public partial class AssembliesPage : MainNavigationConsumerPage
    {
        private readonly AssembliesViewModel _viewModel;

        public AssembliesPage(long buildingId)
        {
            InitializeComponent();

            _viewModel = new AssembliesViewModel(buildingId, this);
            DataContext = _viewModel;

            _viewModel.UpdateAssemblies();
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e)
            => Pop();
    }
}
