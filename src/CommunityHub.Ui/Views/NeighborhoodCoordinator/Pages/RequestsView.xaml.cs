using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Pages
{
    /// <summary>
    /// Interaction logic for RequestsView.xaml
    /// </summary>
    public partial class RequestsView : UserControl
    {
        private long _coordinatorId;
        private readonly NeighborhoodAccessRequestRepository _repository;

        public RequestsView()
        {
            InitializeComponent();
            _repository = new NeighborhoodAccessRequestRepository();
        }

        public void Initialize(long coordinatorId)
        {
            _coordinatorId = coordinatorId;
            LoadData();
        }

        private void LoadData()
        {
            var requests = _repository.GetByCoordinatorId(_coordinatorId);

            var displayData = requests.Select(r =>
            {
                string statusText;
                string statusIcon;

                switch (r.Status)
                {
                    case Status.Accepted:
                        statusText = "Prihvaćen";
                        statusIcon = "\uf00c";
                        break;
                    case Status.Rejected:
                        statusText = "Odbijen";
                        statusIcon = "\uf00d";
                        break;
                    default:
                        statusText = "Čeka odobrenje";
                        statusIcon = "\uf017";
                        break;
                }

                return new
                {
                    OriginalRequest = r,
                    Ime = r.Citizen.Name,
                    Prezime = r.Citizen.Surname,
                    Adresa = $"{r.Citizen.Address.Street.Name} {r.Citizen.Address.StreetNumber}, {r.Citizen.Address.City.Name}",
                    Kvart = r.Neighborhood.Name,
                    Datum = r.CreationDate,
                    StatusIcon = statusIcon,
                    StatusText = statusText
                };
                }).ToList();


            RequestsDataGrid.ItemsSource = displayData;
        }
    }
}
