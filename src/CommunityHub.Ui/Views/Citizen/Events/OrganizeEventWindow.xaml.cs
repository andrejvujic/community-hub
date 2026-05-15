using System.Windows;
using System.Windows.Input;
using CommunityHub.Ui.ViewModels.Citizen.Events;

namespace CommunityHub.Ui.Views.Citizen.Events
{
    public partial class OrganizeEventWindow : Window
    {
        private readonly OrganizeEventViewModel _viewModel;

        public OrganizeEventWindow(long userId)
        {
            InitializeComponent();
            _viewModel = new OrganizeEventViewModel(userId);
            this.DataContext = _viewModel;
        }

        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.AddItem();
        }

        private void ItemInputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                _viewModel.AddItem();
                e.Handled = true;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Submit();
            this.DialogResult = true;
            this.Close();
        }

        private void DiscardButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

