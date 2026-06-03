namespace CommunityHub.Ui.Helpers.BuildingManagerAlerts
{
    public interface IDialogService
    {
        void ShowInfo(string title, string message);
        bool ShowConfirmation(string title, string message);
    }
}