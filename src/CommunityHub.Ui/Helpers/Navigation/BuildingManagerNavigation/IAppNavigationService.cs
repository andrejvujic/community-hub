
namespace CommunityHub.Ui.Navigation
{
    public interface IAppNavigationService
    {
        bool CanGoBack { get; }
        bool CanGoForward { get; }

        AppNavigationState? Current { get; }

        void Navigate(AppNavigationState state);
        void GoBack();
        void GoForward();

        event Action<AppNavigationState>? Navigated;
    }
}