using CommunityHub.Application.Services.Dtos.ResidentAssemblies;
using CommunityHub.Ui.Helpers.Alerts;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident.Buildings.Assemblies;

namespace CommunityHub.Ui.Views.Resident.Buildings.Assemblies.Details
{
    public partial class AssemblyInformationPage : MainNavigationConsumerPage, IAlertServiceConsumer
    {
        public AssemblyInformationPage(long residentId, AssemblyDto assembly)
        {
            InitializeComponent();
            DataContext = new AssemblyInformationViewModel(residentId, assembly, this);
        }
    }
}
