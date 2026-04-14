namespace CommunityHub.Ui.Helpers.Alerts
{
    public class Alert
    {
        public string Title { get; }
        public string Message { get; }
        public AlertType Type { get; }

        public List<AlertOption> Options { get; } = new();

        private Alert(string title, string message, AlertType type)
        {       
            Title = title;
            Message = message;
            Type = type;
        }

        public static Alert Create(string title, string content, AlertType type = AlertType.Info)
            => new Alert(title, content, type);

        public Alert AddOption(string text, Action? callback = null)
        {
            Options.Add(new AlertOption(text, callback));
            return this;
        }
    }
}
