using System.Windows;
using CommunityHub.Application.Domain;

namespace CommunityHub.Ui.Views.Citizen.Events
{
    public partial class OrganizeEventWindow : Window
    {
        private readonly long _userId;

        public OrganizeEventWindow(long userId)
        {
            InitializeComponent();
            _userId = userId;
        }

        private void DiscardButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: implement save
        }
    }
}
