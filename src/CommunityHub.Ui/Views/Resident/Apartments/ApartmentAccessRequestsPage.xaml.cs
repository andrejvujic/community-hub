using CommunityHub.Application.Domain.Apartments.AccessRequests;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident.Buildings.Apartments;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace CommunityHub.Ui.Views.Resident.Apartments
{

    public partial class ApartmentAccessRequestsPage : MainNavigatorConsumerPage
    {

        private readonly ApartmentAccessRequestsViewModel _viewModel;

        public ApartmentAccessRequestsPage(long userId)
        {
            InitializeComponent();
            _viewModel = new ApartmentAccessRequestsViewModel(userId);
            DataContext = _viewModel;
            _viewModel.UpdateRequests();
        }

        private void OpenRequestDetailsPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not FrameworkElement element) return;

            if (element.DataContext is not ApartmentAccessRequest request) return;

            Push(
                new ApartmentAccessRequestDetailsPage(request),
                onPop: _ => _viewModel.UpdateRequests()
            );
        }

        private void OpenSortAndFilterMenu_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            StatusComboBox
                .GetBindingExpression(Selector.SelectedValueProperty)?
                .UpdateTarget();

            OrderComboBox
                .GetBindingExpression(Selector.SelectedItemProperty)?
                .UpdateTarget();

            SortAndFilterOverlay.Visibility = Visibility.Visible;
        }

        private void CancelSortAndFilterButton_Click(object sender, RoutedEventArgs e)
            => SortAndFilterOverlay.Visibility = Visibility.Collapsed;

        private void ApplySortAndFilterButton_Click(object sender, RoutedEventArgs e)
        {
            var statusBinding = StatusComboBox.GetBindingExpression(ComboBox.SelectedValueProperty);
            statusBinding?.UpdateSource();

            var orderBinding = OrderComboBox.GetBindingExpression(ComboBox.SelectedItemProperty);
            orderBinding?.UpdateSource();

            _viewModel.UpdateRequests();

            SortAndFilterOverlay.Visibility = Visibility.Collapsed;
        }
    }
}
