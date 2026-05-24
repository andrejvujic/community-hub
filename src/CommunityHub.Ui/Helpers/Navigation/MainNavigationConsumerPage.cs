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
            {
                window.Push(page, onPop: onPop);
                return;
            }

            // throw new InvalidOperationException("Parent window must inhert from MainNavigationHostWindow");
        }

        public void PushReplacement(IMainNavigationConsumer page)
        {
            if (Window.GetWindow(this) is MainNavigationHostWindow window)
            {
                window.PushReplacement(page);
                return;
            }

            // throw new InvalidOperationException("Parent window must inhert from MainNavigationHostWindow");
        }

        public void Pop(object? returnValue = null)
        {
            if (Window.GetWindow(this) is MainNavigationHostWindow window)
            {
                window.Pop(
                    returnValue: returnValue
                );
                return;
            }


            // throw new InvalidOperationException("Parent window must inhert from MainNavigationHostWindow");
        }

        public void PopAll()
        {
            if (Window.GetWindow(this) is MainNavigationHostWindow window)
            {
                window.PopAll();
                return;
            }

            // throw new InvalidOperationException("Parent window must inhert from MainNavigationHostWindow");
        }
    }
}
