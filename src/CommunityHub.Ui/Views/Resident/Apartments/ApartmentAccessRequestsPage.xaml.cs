using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.Apartments.AccessRequests;
using CommunityHub.Application.Helpers.Sorting;
using CommunityHub.Ui.Helpers.Navigation;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace CommunityHub.Ui.Views.Resident.Apartments
{

    public partial class ApartmentAccessRequestsPage : MainNavigatorConsumerPage, INotifyPropertyChanged
    {

        private readonly long _userId;
        private readonly ApartmentAccessRequestRepository _repository;

        public bool HasRequests { get => Requests.Count > 0; }
        public ObservableCollection<ApartmentAccessRequest> Requests { get; private set; } = [];

        private RequestOrder _selectedOrder = RequestOrder.Unspecified;
        public RequestOrder SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                if (_selectedOrder == value) return;
                _selectedOrder = value;
                OnPropertyChanged(nameof(SelectedOrder));
            }
        }

        public List<StatusOption> StatusOptions { get; } =
        [
            new StatusOption("Svi", value: null),
            new StatusOption("Prihvaćen", value: Status.Accepted),
            new StatusOption("Odbijen", value: Status.Rejected),
            new StatusOption("Čeka odobrenje", value: Status.Waiting)
        ];

        private Status? _selectedStatus;
        public Status? SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                if (_selectedStatus == value) return;
                _selectedStatus = value;
                OnPropertyChanged(nameof(SelectedStatus));
            }
        }

        public ApartmentAccessRequestsPage(long userId)
        {
            InitializeComponent();
            DataContext = this;

            _userId = userId;
            _repository = new ApartmentAccessRequestRepository();

            UpdateRequests();
        }


        public void UpdateRequests()
        {
            var requests = _repository.GetByApplicant(
                _userId,
                status: SelectedStatus,
                order: SelectedOrder
            );

            Requests.Clear();
            foreach (var request in requests)
                Requests.Add(request);

            OnPropertyChanged(nameof(HasRequests));
        }

        private void OpenRequestDetailsPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not FrameworkElement element) return;

            if (element.DataContext is not ApartmentAccessRequest request) return;

            Push(
                new ApartmentAccessRequestDetailsPage(request),
                onPop: _ => UpdateRequests()
            );
        }

        private void OpenSortAndFilterMenu_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            StatusComboBox
                .GetBindingExpression(ComboBox.SelectedValueProperty)?
                .UpdateTarget();

            OrderComboBox
                .GetBindingExpression(ComboBox.SelectedItemProperty)?
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

            UpdateRequests();

            SortAndFilterOverlay.Visibility = Visibility.Collapsed;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
