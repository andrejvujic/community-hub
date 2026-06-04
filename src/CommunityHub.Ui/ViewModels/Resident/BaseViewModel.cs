using CommunityHub.Ui.Helpers.Alerts;
using CommunityHub.Ui.Helpers.Navigation;
using System.ComponentModel;

namespace CommunityHub.Ui.ViewModels.Resident
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        private readonly IMainNavigationConsumer _navigationConsumer;

        public BaseViewModel(IMainNavigationConsumer navigationConsumer)
        {
            _navigationConsumer = navigationConsumer;
        }

        protected void Push(IMainNavigationConsumer target, Action<object?>? onPop = null)
            => _navigationConsumer.Push(target, onPop: onPop);

        protected void PushReplacement(IMainNavigationConsumer target)
            => _navigationConsumer.PushReplacement(target);

        protected void Pop(object? returnValue = null)
            => _navigationConsumer.Pop(returnValue);

        protected void PopAll()
            => _navigationConsumer.PopAll();

        protected void ShowAlert(Alert alert)
        {
            if (_navigationConsumer is IAlertServiceConsumer consumer)
            {
                consumer.ShowAlert(alert);
                return;
            }


            throw new InvalidOperationException("Can't show alert on non IAlertServiceConsumer page");
        }

        protected void CloseAlert()
        {
            if (_navigationConsumer is IAlertServiceConsumer consumer)
            {
                consumer.CloseAlert();
                return;
            }

            throw new InvalidOperationException("Can't close alert on non IAlertServiceConsumer page");
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
