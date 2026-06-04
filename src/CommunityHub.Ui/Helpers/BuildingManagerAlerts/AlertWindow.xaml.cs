using System.Windows;

namespace CommunityHub.Ui.Helpers.BuildingManagerAlerts
{
    public partial class AlertWindow : Window
    {
        public AlertResult AlertResult { get; private set; } = AlertResult.None;
        private readonly AlertType _alertType;

        public AlertWindow(string title, string message, AlertType alertType)
        {
            InitializeComponent();

            _alertType = alertType;

            TitleTextBlock.Text = title;
            MessageTextBlock.Text = message;

            ConfigureByType();
        }
        private void ConfigureByType()
        {
            switch (_alertType)
            {
                case AlertType.Info:
                    Icon.Text = "\uf05a";
                    IconTextBlock.Text = "Informacija";
                    CloseButton.Visibility = Visibility.Visible;
                    CancelButton.Visibility = Visibility.Collapsed;
                    ConfirmButton.Visibility = Visibility.Collapsed;
                    break;

                case AlertType.Confirm:
                    Icon.Text = "\uf06a";
                    IconTextBlock.Text = "Potvrda akcije";
                    CloseButton.Visibility = Visibility.Collapsed;
                    CancelButton.Visibility = Visibility.Visible;
                    ConfirmButton.Visibility = Visibility.Visible;
                    break;
            }
        }
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            AlertResult = AlertResult.Confirmed;
            DialogResult = true;
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            AlertResult = AlertResult.Cancelled;
            DialogResult = false;
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            AlertResult = AlertResult.Closed;
            Close();
        }
        protected override void OnClosed(EventArgs e)
        {
            if (AlertResult == AlertResult.None)
            {
                AlertResult = _alertType == AlertType.Confirm
                    ? AlertResult.Cancelled
                    : AlertResult.Closed;
            }

            base.OnClosed(e);
        }
    }
}