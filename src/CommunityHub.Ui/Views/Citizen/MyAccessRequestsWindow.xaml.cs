using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;
using CommunityHub.Application.Helpers.Sorting;
using System.Linq;
using System.ComponentModel;
using System.Windows.Data;

namespace CommunityHub.Ui.Views.Citizen
{
    public partial class MyAccessRequestsWindow : Window
    {
        public long CitizenId { get; }
        private readonly NeighborhoodAccessRequestRepository _neighborhoodAccessRequestRepository = new NeighborhoodAccessRequestRepository();
        public ObservableCollection<NeighborhoodAccessRequest> Requests { get; } = new ObservableCollection<NeighborhoodAccessRequest>();

        public MyAccessRequestsWindow(long citizenId)
        {
            CitizenId = citizenId;
            DataContext = this;
            InitializeComponent();
            KeyDown += MyAccessRequestsWindow_KeyDown;
            Loaded += MyAccessRequestsWindow_Loaded;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DeleteRequest_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.DataContext is NeighborhoodAccessRequest request)
            {
                var result = MessageBox.Show("Da li ste sigurni da želite da obrišete ovaj zahtev?", "Potvrda brisanja", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result != MessageBoxResult.Yes) return;

                _neighborhoodAccessRequestRepository.Delete(request.Citizen.Id, request.Neighborhood.Id);
                Requests.Remove(request);
            }
        }

        private void MyAccessRequestsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadRequests();
        }

        private void LoadRequests()
        {
            Requests.Clear();
            var items = _neighborhoodAccessRequestRepository.GetByCitizenId(CitizenId);
            foreach (var r in items) Requests.Add(r);
            ApplySort(RequestOrder.CreationDateDescending);
        }

        private void NewestToggle_Checked(object sender, RoutedEventArgs e)
        {
            ApplySort(RequestOrder.CreationDateDescending);
        }

        private void OldestToggle_Checked(object sender, RoutedEventArgs e)
        {
            ApplySort(RequestOrder.CreationDateAscending);
        }

        private void GroupByStatusToggle_Checked(object sender, RoutedEventArgs e)
        {
            var order = NewestToggle?.IsChecked == true
                ? RequestOrder.CreationDateDescending
                : RequestOrder.CreationDateAscending;
            ApplySort(order);
        }

        private void GroupByStatusToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            var order = NewestToggle?.IsChecked == true
                ? RequestOrder.CreationDateDescending
                : RequestOrder.CreationDateAscending;
            ApplySort(order);
        }

        private void ApplySort(RequestOrder order)
        {
            var view = CollectionViewSource.GetDefaultView(Requests);
            if (view == null) return;

            view.SortDescriptions.Clear();
            var direction = order == RequestOrder.CreationDateAscending ? ListSortDirection.Ascending : ListSortDirection.Descending;
            view.SortDescriptions.Add(new SortDescription(nameof(NeighborhoodAccessRequest.CreationDate), direction));

            view.GroupDescriptions.Clear();
            if (GroupByStatusToggle?.IsChecked == true)
                view.GroupDescriptions.Add(new PropertyGroupDescription(nameof(NeighborhoodAccessRequest.Status)));
        }

        private void MyAccessRequestsWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                if (e.Key == Key.D1 || e.Key == Key.NumPad1)
                {
                    if (NewestToggle != null && OldestToggle != null)
                    {
                        if (NewestToggle.IsChecked == true)
                            OldestToggle.IsChecked = true;
                        else
                            NewestToggle.IsChecked = true;
                    }
                    e.Handled = true;
                    return;
                }

                if (e.Key == Key.D2 || e.Key == Key.NumPad2)
                {
                    if (GroupByStatusToggle != null)
                    {
                        GroupByStatusToggle.IsChecked = !(GroupByStatusToggle.IsChecked == true);
                    }
                    e.Handled = true;
                    return;
                }
            }
        }

    }
}
