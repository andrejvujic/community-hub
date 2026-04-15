using System.Windows;

namespace CommunityHub.Ui.Views.Citizen
{
    public partial class MyAccessRequestsWindow : Window
    {
        public long CitizenId { get; }

        public MyAccessRequestsWindow(long citizenId)
        {
            CitizenId = citizenId;
            DataContext = this;
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DeleteRequest_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
