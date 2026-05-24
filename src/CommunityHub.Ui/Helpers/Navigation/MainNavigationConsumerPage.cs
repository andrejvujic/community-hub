using System.Windows;
using System.Windows.Controls;

namespace CommunityHub.Ui.Helpers.Navigation
{
    public class MainNavigationConsumerPage : Page, IMainNavigationConsumer
    {
        public string Id { get; } = Guid.NewGuid().ToString();

        public void Push(IMainNavigationConsumer page, Action<object?>? onPop = null)
        {
            if (Window.GetWindow(this) is MainNavigationHostWindow window)
                window.Push(page, onPop: onPop);
        }

        public void PushReplacement(IMainNavigationConsumer page)
        {
            if (Window.GetWindow(this) is IMainNavigationConsumer window)
                window.PushReplacement(page);
        }

        public void Pop(object? returnValue = null)
        {
            if (Window.GetWindow(this) is IMainNavigationConsumer window)
                window.Pop(
                    returnValue: returnValue
                );
        }

        public void PopAll()
        {
            if (Window.GetWindow(this) is IMainNavigationConsumer window)
                window.PopAll();
        }
    }
}
