using CommunityHub.Ui.Helpers.Commands;
using System.ComponentModel;
using System.Windows.Input;

namespace CommunityHub.Ui.ViewModels.Citizen.Help
{
    public class QuickHelpViewModel : INotifyPropertyChanged
    {
        private string _pageName = "";
        private string _description = "";
        private List<KeyboardShortcut> _shortcuts = new();

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler? CloseRequested;

        public string PageName
        {
            get => _pageName;
            set
            {
                if (_pageName == value) return;
                _pageName = value;
                OnPropertyChanged(nameof(PageName));
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                if (_description == value) return;
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public List<KeyboardShortcut> Shortcuts
        {
            get => _shortcuts;
            set
            {
                if (_shortcuts == value) return;
                _shortcuts = value;
                OnPropertyChanged(nameof(Shortcuts));
            }
        }

        public ICommand CloseCommand { get; }

        public QuickHelpViewModel()
        {
            CloseCommand = new RelayCommand<object>(_ => CloseRequested?.Invoke(this, EventArgs.Empty));
        }

        public void SetContent(string pageName, string description, List<KeyboardShortcut> shortcuts)
        {
            PageName = pageName;
            Description = description;
            Shortcuts = shortcuts;
        }

        protected virtual void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class KeyboardShortcut
    {
        public string Keys { get; set; } = "";
        public string Description { get; set; } = "";
    }
}