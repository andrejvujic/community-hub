using System.Windows;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Windows
{
    /// <summary>
    /// Interaction logic for InfoDialog.xaml
    /// </summary>
    public partial class InfoDialog : Window
    {
        public InfoDialog(string title, string message, Window owner)
        {
            InitializeComponent();
            TitleText.Text = title;
            MessageText.Text = message;
            Owner = owner;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e) => Close();
    }
}
