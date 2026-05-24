namespace CommunityHub.Ui.Helpers.Navigation
{
    public interface IMainNavigationConsumer
    {
        public abstract string Id { get; }

        public void Push(IMainNavigationConsumer page, Action<object?>? onPop = null);

        public void PushReplacement(IMainNavigationConsumer page);

        public void Pop(object? returnValue = null);

        public void PopAll();
    }
}
