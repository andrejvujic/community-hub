using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CommunityHub.Ui.Helpers
{
    public class TooltipStateProxy : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public bool IsEnabled
        {
            get => TooltipState.IsEnabled;
            set => TooltipState.IsEnabled = value;
        }

        public TooltipStateProxy()
        {
            TooltipState.OnPropertyChanged += (s, e) =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsEnabled)));
        }
    }
}