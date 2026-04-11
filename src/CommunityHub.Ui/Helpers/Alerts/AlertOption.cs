using System;
using System.Collections.Generic;
using System.Text;

namespace CommunityHub.Ui.Helpers.Alerts
{
    public class AlertOption
    {
        public string Text;
        public Action? Callback;
        public AlertOption(string text, Action? callback = null)
        {
            Text = text;
            Callback = callback;
        }
    }
}
