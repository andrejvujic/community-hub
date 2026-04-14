
namespace CommunityHub.Ui.Helpers.Navigation
{
    public class NavigationEntry
    {
        public string PageId { get; set; } = string.Empty;
        public Action<object?>? OnPop;

        public NavigationEntry(string pageId, Action<object?>? onPop = null)
        {
            PageId = pageId;
            OnPop = onPop;
        }
    }
}
