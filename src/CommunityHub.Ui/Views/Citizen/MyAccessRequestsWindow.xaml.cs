using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Citizen
{
    public partial class MyAccessRequestsWindow : Window
    {
        public long CitizenId { get; }

        public MyAccessRequestsWindow(long citizenId)
        {
            CitizenId = citizenId;
            DataContext = this;
            InitializeComponent();
            KeyDown += MyAccessRequestsWindow_KeyDown;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DeleteRequest_Click(object sender, RoutedEventArgs e)
        {
        }

        private void MyAccessRequestsWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                if (e.Key == Key.D1 || e.Key == Key.NumPad1)
                {
                    if (NewestToggle != null && OldestToggle != null)
                    {
                        if (NewestToggle.IsChecked == true)
                            OldestToggle.IsChecked = true;
                        else
                            NewestToggle.IsChecked = true;
                    }
                    e.Handled = true;
                    return;
                }

                if (e.Key == Key.D2 || e.Key == Key.NumPad2)
                {
                    if (GroupByStatusToggle != null)
                    {
                        GroupByStatusToggle.IsChecked = !(GroupByStatusToggle.IsChecked == true);
                    }
                    e.Handled = true;
                    return;
                }
            }
        }

    }
}
