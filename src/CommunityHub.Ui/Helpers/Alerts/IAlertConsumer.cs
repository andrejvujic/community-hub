using System.Windows;
using System.Windows.Controls;

namespace CommunityHub.Ui.Helpers.Alerts
{
    public interface IAlertConsumer
    {
        protected Page Page => (Page)this;
        
        public void ShowAlert(Alert alert)
        {
            if (Window.GetWindow(Page) is IAlertHost host)
                host.ShowAlert(alert);
        }

        public void CloseAlert()
        {
            if (Window.GetWindow(Page) is IAlertHost host)
                host.CloseAlert();               
        }
    }
}
