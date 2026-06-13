using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Tabs;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Tabs
{
    public partial class StatisticsAndMeetingsTab : UserControl
    {
        private readonly StatisticsAndMeetingsTabViewModel _viewModel;

        public StatisticsAndMeetingsTab()
        {
            InitializeComponent();
            Language = XmlLanguage.GetLanguage("sr-Latn-RS");

            _viewModel = new StatisticsAndMeetingsTabViewModel();
            DataContext = _viewModel;

            this.IsVisibleChanged += StatisticsAndMeetingsTab_IsVisibleChanged;
        }

        public void Initialize(long coordinatorId)
        {
            _viewModel.Initialize(coordinatorId);
            Window.GetWindow(this).KeyDown += StatisticsAndMeetingsTab_KeyDown;
        }

        private void StatisticsAndMeetingsTab_KeyDown(object sender, KeyEventArgs e)
        {
            if (!IsVisible) return;
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && e.Key == Key.N)
            {
                if (_viewModel.ScheduleMeetingCommand.CanExecute(null))
                    _viewModel.ScheduleMeetingCommand.Execute(null);
            }
        }

        private void StatisticsAndMeetingsTab_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                _viewModel.LoadNeighborhoods();
                _viewModel.LoadMeetingsAndStatistics();
            }
        }
    }
}