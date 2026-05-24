
namespace CommunityHub.Ui.Navigation
{
    public class AppNavigationService : IAppNavigationService
    {
        private readonly List<AppNavigationState> _history = new();
        private int _currentIndex = -1;

        public bool CanGoBack => _currentIndex > 0;
        public bool CanGoForward => _currentIndex < _history.Count - 1;

        public AppNavigationState? Current =>
            _currentIndex >= 0 && _currentIndex < _history.Count
                ? _history[_currentIndex]
                : null;

        public event Action<AppNavigationState>? Navigated;

        public void Navigate(AppNavigationState state)
        {
            if (_currentIndex < _history.Count - 1)
            {
                _history.RemoveRange(_currentIndex + 1, _history.Count - _currentIndex - 1);
            }

            _history.Add(state);
            _currentIndex++;

            Navigated?.Invoke(state);
        }

        public void GoBack()
        {
            if (!CanGoBack)
                return;

            _currentIndex--;
            Navigated?.Invoke(_history[_currentIndex]);
        }

        public void GoForward()
        {
            if (!CanGoForward)
                return;

            _currentIndex++;
            Navigated?.Invoke(_history[_currentIndex]);
        }
    }
}