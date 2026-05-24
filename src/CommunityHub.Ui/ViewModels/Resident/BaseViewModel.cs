using CommunityHub.Ui.Helpers.Alerts;
using CommunityHub.Ui.Helpers.Navigation;
using System.ComponentModel;

namespace CommunityHub.Ui.ViewModels.Resident
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        private readonly MainNavigationConsumerPage _page;

        public BaseViewModel(MainNavigationConsumerPage page)
        {
            _page = page;
        }

        protected void Push(MainNavigationConsumerPage page, Action<object?>? onPop = null)
            => _page.Push(page, onPop: onPop);

        protected void PushReplacement(MainNavigationConsumerPage page)
            => _page.PushReplacement(page);

        protected void Pop(object? returnValue = null)
            => _page.Pop(returnValue);

        protected void PopAll()
            => _page.PopAll();

        protected void ShowAlert(Alert alert)
        {
            if (_page is IAlertServiceConsumer consumer)
            {
                consumer.ShowAlert(alert);
                return;
            }


            throw new InvalidOperationException("Can't show alert on non IAlertServiceConsumer page");
        }

        protected void CloseAlert()
        {
            if (_page is IAlertServiceConsumer consumer)
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
