using CommunityHub.Application.Services.Dtos.Buildings;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident.Buildings.Browse;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.Browse
{
    public partial class BuildingDetailsPage : MainNavigatorConsumerPage
    {
        private readonly BuildingDetailsViewModel _viewModel;

        public BuildingDetailsPage(BuildingDto building)
        {
            InitializeComponent();
            _viewModel = new BuildingDetailsViewModel(building);
            DataContext = _viewModel;
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e) => Pop();

        private void PreviousImageButton_Click(object sender,RoutedEventArgs e)
            => _viewModel.GoToPreviousImage();

        private void NextImageButton_Click(object sender, RoutedEventArgs e)
            => _viewModel.GoToNextImage();
    }
}
