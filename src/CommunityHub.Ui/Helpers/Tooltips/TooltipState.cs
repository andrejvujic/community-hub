namespace CommunityHub.Ui.Helpers
{
    public static class TooltipState
    {
        private static bool _isEnabled = true;

        public static bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                OnPropertyChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        public static event EventHandler? OnPropertyChanged;
    }
}