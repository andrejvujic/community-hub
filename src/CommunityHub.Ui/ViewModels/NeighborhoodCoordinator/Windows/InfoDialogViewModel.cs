using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Views.NeighborhoodCoordinator.Windows;

namespace CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Windows
{
    public class InfoDialogViewModel : INotifyPropertyChanged
    {
        public string Title { get; }
        public string Message { get; }

        public RelayCommand OkCommand { get; }

        public InfoDialogViewModel(string title, string message)
        {
            Title = title;
            Message = message;
            OkCommand = new RelayCommand(ExecuteOk);
        }

        private void ExecuteOk() => CloseWindow();

        private void CloseWindow()
        {
            var window = System.Windows.Application.Current.Windows
                .OfType<Window>().FirstOrDefault(w => w.IsActive);

            if (window is InfoDialog dialog)
            {
                dialog.Close();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}