using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Views.NeighborhoodCoordinator.Windows;

namespace CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Windows
{
    public class ConfirmDialogViewModel : INotifyPropertyChanged
    {
        public string Title { get; }
        public string Message { get; }

        public RelayCommand YesCommand { get; }
        public RelayCommand NoCommand { get; }

        public ConfirmDialogViewModel(string title, string message)
        {
            Title = title;
            Message = message;
            YesCommand = new RelayCommand(ExecuteYes);
            NoCommand = new RelayCommand(ExecuteNo);
        }

        private void ExecuteYes() => CloseWindow(true);

        private void ExecuteNo() => CloseWindow(false);

        private void CloseWindow(bool result)
        {
            var window = System.Windows.Application.Current.Windows
                .OfType<Window>().FirstOrDefault(w => w.IsActive);

            if (window is ConfirmDialog dialog)
            {
                dialog.DialogResult = result;
                dialog.Close();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}