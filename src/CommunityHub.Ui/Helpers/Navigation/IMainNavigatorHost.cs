using System.Windows.Controls;

namespace CommunityHub.Ui.Helpers.Navigation
{
    public interface IMainNavigatorHost
    {
        public abstract Frame HostFrame { get; }

        public void Push(Page page) => HostFrame?.Navigate(page);

        public void Pop() {
            if (HostFrame == null || !HostFrame.CanGoBack)
                return;
            HostFrame.GoBack();
        }
    }
}
