using System.Windows;
using System.Windows.Controls;

namespace CommunityHub.Ui.Helpers.Navigation
{
    public class MainNavigationConsumerPage : Page
    {
        public string Id { get; } = Guid.NewGuid().ToString();

        protected void Push(MainNavigationConsumerPage page, Action<object?>? onPop = null)
        {
            if (Window.GetWindow(this) is MainNavigationHostWindow window)
                window.Push(page, onPop: onPop);
        }

        protected void PushReplacement(MainNavigationConsumerPage page)
        {
            if (Window.GetWindow(this) is MainNavigationHostWindow window)
                window.PushReplacement(page);
        }

        protected void Pop(object? returnValue = null)
        {
            if (Window.GetWindow(this) is MainNavigationHostWindow window)
                window.Pop(
                    returnValue: returnValue
                );
        }

        protected void PopAll()
        {
            if (Window.GetWindow(this) is MainNavigationHostWindow window)
                window.PopAll();
        }
    }
}
