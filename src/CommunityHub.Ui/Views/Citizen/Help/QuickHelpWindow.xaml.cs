using CommunityHub.Ui.ViewModels.Citizen.Help;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Citizen.Help
{
    public partial class QuickHelpWindow : Window
    {
        private QuickHelpViewModel _viewModel;

        public QuickHelpWindow()
        {
            InitializeComponent();
            _viewModel = new QuickHelpViewModel();
            _viewModel.CloseRequested += (_, _) => Close();
            DataContext = _viewModel;
        }

        public QuickHelpWindow(string pageName, string description, List<KeyboardShortcut> shortcuts) : this()
        {
            _viewModel.SetContent(pageName, description, shortcuts);
        }

        private void Header_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) DragMove();
        }
    }
}