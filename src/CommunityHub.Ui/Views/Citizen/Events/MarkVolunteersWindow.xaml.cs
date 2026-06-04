using System.Windows;
using System.Windows.Input;
using CommunityHub.Application.Services.Dtos.Events;
using CommunityHub.Ui.ViewModels.Citizen.Events;

namespace CommunityHub.Ui.Views.Citizen.Events
{
    public partial class MarkVolunteersWindow : Window
    {
        private MarkVolunteersViewModel? _viewModel;

        public MarkVolunteersWindow(CommunityEventDto eventDto)
        {
            InitializeComponent();
            _viewModel = new MarkVolunteersViewModel(eventDto);
            DataContext = _viewModel;

            if (_viewModel != null)
            {
                _viewModel.CloseRequested += (_, _) => Close();
            }

            Loaded += (_, _) => _viewModel?.SelectFirstRegistration();
        }

        private void Header_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        protected override void OnClosed(System.EventArgs e)
        {
            base.OnClosed(e);
            _viewModel?.MarkUnattendedAsAbsent();
        }
    }
}
