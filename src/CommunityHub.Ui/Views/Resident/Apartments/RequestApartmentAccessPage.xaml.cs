using CommunityHub.Application.Services.Apartments;
using CommunityHub.Application.Services.Dtos.Buildings;
using CommunityHub.Ui.Helpers.Alerts;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident.Buildings.Apartments;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Apartments
{
    public partial class RequestApartmentAccessPage : MainNavigationConsumerPage, IAlertServiceConsumer
    {
        private readonly RequestApartmentAccessViewModel _viewModel;

        public RequestApartmentAccessPage(long userId, BuildingDto building)
        {
            InitializeComponent();
            _viewModel = new RequestApartmentAccessViewModel(userId, building, this);
            DataContext = _viewModel;
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e) => Pop();
        private void CreateAccessRequestButton_Click(object sender, RoutedEventArgs e)
            => _viewModel.SubmitAccessRequest();
    }
}
