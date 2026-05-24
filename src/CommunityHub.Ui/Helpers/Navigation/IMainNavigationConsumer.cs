namespace CommunityHub.Ui.Helpers.Navigation
{
    public interface IMainNavigationConsumer
    {
        public abstract string Id { get; }

        public void Push(IMainNavigationConsumer target, Action<object?>? onPop = null);

        public void PushReplacement(IMainNavigationConsumer target);

        public void Pop(object? returnValue = null);

        public void PopAll();
    }
}
