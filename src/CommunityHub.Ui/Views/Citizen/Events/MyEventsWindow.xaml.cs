using System.Windows;
using System.Windows.Input;
using CommunityHub.Application.Services.Dtos.Events;
using CommunityHub.Ui.ViewModels.Citizen.Events;

namespace CommunityHub.Ui.Views.Citizen.Events
{
    public partial class MyEventsWindow : Window
    {
        private MyEventsViewModel? _viewModel;

        public MyEventsWindow(long userId)
        {
            InitializeComponent();
            _viewModel = new MyEventsViewModel(userId);
            this.DataContext = _viewModel;

            if (_viewModel != null)
            {
                _viewModel.CloseRequested += (_, _) => Close();
                _viewModel.OrganizeEventRequested += (_, _) => HandleOrganizeEvent();
                _viewModel.OpenMarkVolunteersRequested += (_, eventDto) => HandleOpenMarkVolunteers(eventDto);
            }
        }

        private void Header_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void HandleOrganizeEvent()
        {
            var wnd = new OrganizeEventWindow(_viewModel!.UserId);
            wnd.Owner = this;
            var dialogResult = wnd.ShowDialog();
            if (dialogResult == true)
            {
                _viewModel.LoadEvents();
                this.DialogResult = true;
            }
        }

        private void HandleOpenMarkVolunteers(CommunityEventDto eventDto)
        {
            var wnd = new MarkVolunteersWindow(eventDto);
            wnd.Owner = this;
            wnd.ShowDialog();
            _viewModel?.LoadEvents();
        }
    }
}
