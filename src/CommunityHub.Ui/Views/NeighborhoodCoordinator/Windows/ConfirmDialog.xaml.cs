using System.Windows;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Windows
{
    /// <summary>
    /// Interaction logic for ConfirmDialog.xaml
    /// </summary>
    public partial class ConfirmDialog : Window
    {
        public bool Confirmed { get; private set; }

        public ConfirmDialog(string title, string message, Window owner)
        {
            InitializeComponent();
            TitleText.Text = title;
            MessageText.Text = message;
            Owner = owner;
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            Confirmed = true;
            Close();
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            Confirmed = false;
            Close();
        }
    }
}
