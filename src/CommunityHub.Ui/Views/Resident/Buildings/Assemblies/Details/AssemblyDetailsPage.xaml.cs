using CommunityHub.Application.Services.Dtos.ResidentAssemblies;
using CommunityHub.Ui.Helpers.Navigation;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.Assemblies.Details
{
    public partial class AssemblyDetailsPage : MainNavigationConsumerPage
    {
        private readonly long _residentId;
        public AssemblyDto Assembly { get; }

        public AssemblyDetailsPage(long residentId, AssemblyDto assembly)
        {
            _residentId = residentId;
            Assembly = assembly;

            InitializeComponent();
            DataContext = this;

            OpenAssemblyInformationPage();
        }

        private void OpenAssemblyInformationPage()
            => LocalFrame.Navigate(new AssemblyInformationPage(_residentId, Assembly));

        private void OpenAssemblyTopicsPage()
            => LocalFrame.Navigate(new AssemblyTopicsPage());

        private void BackButton_Click(object sender, MouseButtonEventArgs e) => Pop();

        private void BottomBar_InformationItemClick(object sender, MouseButtonEventArgs e)
            => OpenAssemblyInformationPage();

        private void BottomBar_TopicsItemClick(object sender, MouseButtonEventArgs e)
            => OpenAssemblyTopicsPage();
    }
}
