using CommunityHub.Ui.ViewModels.Citizen.Help;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Citizen.Help
{
    public partial class HelpWizardWindow : Window
    {
        private HelpWizardViewModel _viewModel;

        public HelpWizardWindow()
        {
            InitializeComponent();
            _viewModel = new HelpWizardViewModel();
            _viewModel.CloseRequested += (_, _) => Close();
            DataContext = _viewModel;
        }

        public HelpWizardWindow(List<WizardStep> steps) : this()
        {
            _viewModel.SetSteps(steps);
        }

        private void Header_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}