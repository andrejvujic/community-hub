using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;
using CommunityHub.Application.Services;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Pages
{
    /// <summary>
    /// Interaction logic for ServiceCenterTab.xaml
    /// </summary>
    public partial class ServiceCenterTab : UserControl
    {
        private long _coordinatorId;
        private Neighborhood? _selectedNeighborhood;
        private JobListing? _selectedListing;

        private readonly NeighborhoodRepository _neighborhoodRepository = new NeighborhoodRepository();
        private readonly JobBookingService _jobBookingService = new(
            new JobListingRepository(),
            new BookedJobRepository()
        );

        public ObservableCollection<Neighborhood> Neighborhoods { get; set; } = new ObservableCollection<Neighborhood>();
        public ObservableCollection<JobListing> JobListings { get; set; } = new();

        public ServiceCenterTab()
        {
            InitializeComponent();
            DataContext = this;
        }

        public void Initialize(long coordinatorId)
        {
            _coordinatorId = coordinatorId;
            LoadNeighborhoods();
        }

        private void LoadNeighborhoods()
        {
            Neighborhoods.Clear();
            var neighborhoods = _neighborhoodRepository.GetByCoordinatorId(_coordinatorId);
            foreach (var n in neighborhoods)
                Neighborhoods.Add(n);
        }

        private void NeighborhoodsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedNeighborhood = NeighborhoodsListBox.SelectedItem as Neighborhood;
            if (_selectedNeighborhood is null) return;

            JobListings.Clear();

            ServicesTitleTextBlock.Text =
                $"Dostupne servisne usluge za kvart {_selectedNeighborhood.Name} " +
                $"(lokacija {_selectedNeighborhood.City.Name}, {_selectedNeighborhood.Country.Name})";

            var jobListings = _jobBookingService.GetJobListingsForLocation(_selectedNeighborhood.City.Id, _selectedNeighborhood.Country.Id);

            foreach (var jobListing in jobListings)
                JobListings.Add(jobListing);

            NeighborhoodServiceDetails.Visibility = Visibility.Visible;
        }
    }
}
