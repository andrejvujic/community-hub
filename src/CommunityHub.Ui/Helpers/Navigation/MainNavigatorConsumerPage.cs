using System.Windows;
using System.Windows.Controls;

namespace CommunityHub.Ui.Helpers.Navigation
{
    public class MainNavigatorConsumerPage : Page
    {
        public string Id { get; } = Guid.NewGuid().ToString();

        protected void Push(MainNavigatorConsumerPage page, Action<object?>? onPop = null)
        {
            if (Window.GetWindow(this) is MainNavigatorHostWindow window)
                window.Push(page, onPop: onPop);
        }

        protected void PushReplacement(MainNavigatorConsumerPage page)
        {
            if (Window.GetWindow(this) is MainNavigatorHostWindow window)
                window.PushReplacement(page);
        }

        protected void Pop(object? returnValue = null)
        {
            if (Window.GetWindow(this) is MainNavigatorHostWindow window)
                window.Pop(
                    returnValue: returnValue
                );
        }

        protected void PopAll()
        {
            if (Window.GetWindow(this) is MainNavigatorHostWindow window)
                window.PopAll();
        }
    }
}
