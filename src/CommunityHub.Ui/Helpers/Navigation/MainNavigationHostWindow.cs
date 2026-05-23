using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace CommunityHub.Ui.Helpers.Navigation;

    public abstract class MainNavigationHostWindow : Window
    {
        public abstract Frame Frame { get; }

        private readonly Stack<NavigationEntry> _entries = new();

        public void Push(MainNavigationConsumerPage page, Action<object?>? onPop = null)
        {
            Frame.Navigate(page);
            _entries.Push(
                new NavigationEntry(
                        page.Id,
                        onPop: onPop
                    )
                );
        }

        public void PushReplacement(MainNavigationConsumerPage page)
        {
            Frame.Navigate(page);

            if (Frame.CanGoBack) {
                Frame.RemoveBackEntry();
                if (_entries.Count > 0)
                    _entries.Pop(); // TODO: check if onPop should should be called here
        }

            _entries.Push(new NavigationEntry(page.Id));
    }

        public void Pop(object? returnValue = null)
        {
            if (Frame == null || !Frame.CanGoBack)
                    return;

            Frame.GoBack();

            if (_entries.Count > 0)
            {
                var entry = _entries.Pop();
                Frame.Dispatcher.BeginInvoke(
                    DispatcherPriority.Background,
                    new Action(() => entry.OnPop?.Invoke(returnValue))
                );
            }
        }

    public void PopAll()
        {
            if (Frame == null) return;
            while (Frame.CanGoBack)
                Pop();
        }
    }
