using System.Windows;
using System.Windows.Controls;

namespace CommunityHub.Ui.Helpers.Alerts
{
    public interface IAlertServiceHost
    {
        public FrameworkElement Root { get; }
        public TextBlock TypeTextBlock { get; }
        public TextBlock TitleTextBlock { get; }
        public TextBlock MessageTextBlock { get; }
        public Panel OptionsPanel { get; }

        private void ResetAlert()
        {
            TitleTextBlock.Text = string.Empty;
            MessageTextBlock.Text = string.Empty;
            OptionsPanel.Children.Clear();
        }

        private void ApplyAlert(Alert alert)
        {
            SetType(alert.Type);
            SetTitle(alert.Title);
            SetMessage(alert.Message);

            foreach (var option in alert.Options)
                AddOption(option);
        }

        private void SetTitle(string title) => TitleTextBlock.Text = title;

        private void SetMessage(string message) => MessageTextBlock.Text = message;

        private void SetType(AlertType type)
        {
            switch (type)
            {
                case AlertType.Info:
                    TypeTextBlock.Text = "\uf05a";
                    break;
                case AlertType.Question:
                    TypeTextBlock.Text = "\uf059";
                    break;
                case AlertType.Warning:
                    TypeTextBlock.Text = "\uf071";
                    break;
                case AlertType.Error:
                    TypeTextBlock.Text = "\uf057";
                    break;
            }
        }

        private void AddOption(AlertOption option)
        {
            Button button = new Button();

            button.Height = 48;
            button.FontSize = 18;
            button.Margin = OptionsPanel.Children.Count > 0 ? new Thickness(8, 0, 0, 0) : new Thickness(0);

            button.Content = option.Text;
            button.Click += (s, e) => {
                CloseAlert();
                option.Callback?.Invoke();
            };

            OptionsPanel.Children.Add(button);
        }

        public void ShowAlert(Alert alert)
        {
            ResetAlert();
            ApplyAlert(alert);
            Root.Visibility = Visibility.Visible;
        }

        public void CloseAlert()
            => Root.Visibility = Visibility.Collapsed;
    }
}
