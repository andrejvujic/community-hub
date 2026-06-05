using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Views.NeighborhoodCoordinator.Windows;

namespace CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Windows
{
    public class RejectRequestWindowViewModel : INotifyPropertyChanged
    {
        public string MessageText { get; }

        private string _comment = string.Empty;
        public string Comment
        {
            get => _comment;
            set 
            { 
                _comment = value; 
                OnPropertyChanged(); 
            }
        }

        public RelayCommand ConfirmCommand { get; }
        public RelayCommand CancelCommand { get; }

        public RejectRequestWindowViewModel(string ime, string prezime, string kvart)
        {
            MessageText = $"Napišite razlog odbijanja zahteva korisnika {ime} {prezime} za pristup kvartu {kvart}:";
            ConfirmCommand = new RelayCommand(ExecuteConfirm);
            CancelCommand = new RelayCommand(ExecuteCancel);
        }

        private void ExecuteConfirm() => CloseWindow(true);

        private void ExecuteCancel() => CloseWindow(false);

        private void CloseWindow(bool result)
        {
            var window = System.Windows.Application.Current.Windows
                .OfType<Window>().FirstOrDefault(w => w.IsActive);

            if (window is RejectRequestWindow view)
            {
                view.DialogResult = result;
                view.Close();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}