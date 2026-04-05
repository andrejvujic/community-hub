using System.Windows;
using System.Windows.Controls;

namespace CommunityHub.Ui.Helpers.Navigation
{
    public class MainNavigatorConsumerPage : Page
    {
        protected void Push(Page page)
        {
            if (Window.GetWindow(this) is IMainNavigatorHost window)
                window.Push(page);
        }
        protected void Pop()
        {
            if (Window.GetWindow(this) is IMainNavigatorHost window)
                window.Pop();
        }

    }
}
