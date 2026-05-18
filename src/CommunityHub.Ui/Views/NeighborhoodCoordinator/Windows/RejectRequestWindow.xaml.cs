using System.Windows;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Windows
{
    public partial class RejectRequestWindow : Window
    {
        public string Comment { get; private set; }

        public RejectRequestWindow(string ime, string prezime, string kvart)
        {
            InitializeComponent();
            MessageLabel.Text = $"Napišite razlog odbijanja zahteva korisnika {ime} {prezime} za pristup kvartu {kvart}:";
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            Comment = CommentTextBox.Text;
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}