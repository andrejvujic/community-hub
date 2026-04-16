using System.Windows;

namespace CommunityHub.Ui.Views.Citizen
{
    public partial class RequestNeighborhoodAccessWindow : Window
    {
        public bool Confirmed { get; private set; }
        public string NeighborhoodName { get; }

        public RequestNeighborhoodAccessWindow(string neighborhoodName)
        {
            NeighborhoodName = neighborhoodName;
            DataContext = this;
            InitializeComponent();
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            Confirmed = true;
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Confirmed = false;
            DialogResult = false;
            Close();
        }
    }
}
