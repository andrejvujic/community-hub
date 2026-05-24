using CommunityHub.Ui.Helpers.Alerts;
using CommunityHub.Ui.Helpers.Navigation;
using System.ComponentModel;

namespace CommunityHub.Ui.ViewModels.Resident
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        private readonly IMainNavigationConsumer _target;

        public BaseViewModel(IMainNavigationConsumer target)
        {
            _target = target;
        }

        protected void Push(IMainNavigationConsumer page, Action<object?>? onPop = null)
            => _target.Push(page, onPop: onPop);

        protected void PushReplacement(IMainNavigationConsumer page)
            => _target.PushReplacement(page);

        protected void Pop(object? returnValue = null)
            => _target.Pop(returnValue);

        protected void PopAll()
            => _target.PopAll();

        protected void ShowAlert(Alert alert)
        {
            if (_target is IAlertServiceConsumer consumer)
            {
                consumer.ShowAlert(alert);
                return;
            }


            throw new InvalidOperationException("Can't show alert on non IAlertServiceConsumer page");
        }

        protected void CloseAlert()
        {
            if (_target is IAlertServiceConsumer consumer)
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
