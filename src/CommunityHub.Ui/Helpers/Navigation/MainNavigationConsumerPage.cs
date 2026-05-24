using System.Windows;
using System.Windows.Controls;

namespace CommunityHub.Ui.Helpers.Navigation
{
    public class MainNavigationConsumerPage : Page
    {
        public string Id { get; } = Guid.NewGuid().ToString();

        public void Push(MainNavigationConsumerPage page, Action<object?>? onPop = null)
        {
            if (Window.GetWindow(this) is MainNavigationHostWindow window)
                window.Push(page, onPop: onPop);
        }

        public void PushReplacement(MainNavigationConsumerPage page)
        {
            if (Window.GetWindow(this) is MainNavigationHostWindow window)
                window.PushReplacement(page);
        }

        public void Pop(object? returnValue = null)
        {
            if (Window.GetWindow(this) is MainNavigationHostWindow window)
                window.Pop(
                    returnValue: returnValue
                );
        }

        public void PopAll()
        {
            if (Window.GetWindow(this) is MainNavigationHostWindow window)
                window.PopAll();
        }
    }
}
