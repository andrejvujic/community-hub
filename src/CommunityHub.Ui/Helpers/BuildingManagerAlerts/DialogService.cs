namespace CommunityHub.Ui.Helpers.BuildingManagerAlerts
{
    public class DialogService : IDialogService
    {
        public void ShowInfo(string title, string message)
        {
            var dialog = new AlertWindow(title, message, AlertType.Info);
            dialog.Owner = App.Current.MainWindow;
            dialog.ShowDialog();
        }

        public bool ShowConfirmation(string title, string message)
        {
            var dialog = new AlertWindow(title, message, AlertType.Confirm);
            dialog.Owner = App.Current.MainWindow;
            return dialog.ShowDialog() == true;
        }
    }
}