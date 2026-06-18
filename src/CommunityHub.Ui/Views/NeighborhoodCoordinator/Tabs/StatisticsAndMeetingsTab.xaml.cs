using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using CommunityHub.Ui.Resources.Localization;
using CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Tabs;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Tabs
{
    public partial class StatisticsAndMeetingsTab : UserControl
    {
        private readonly StatisticsAndMeetingsTabViewModel _viewModel;

        public StatisticsAndMeetingsTab()
        {
            InitializeComponent();

            _viewModel = new StatisticsAndMeetingsTabViewModel();
            DataContext = _viewModel;

            this.IsVisibleChanged += StatisticsAndMeetingsTab_IsVisibleChanged;

            TranslationSource.Instance.PropertyChanged += (s, e) =>
            {
                if (this.IsVisible)
                {
                    RefreshTabLanguageAndTables();
                }
            };
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
                long? previousSelectedId = _viewModel.SelectedMeeting?.Id;

                _viewModel.LoadNeighborhoods();
                _viewModel.LoadMeetingsAndStatistics();

                if (previousSelectedId.HasValue)
                {
                    _viewModel.SelectedMeeting = System.Linq.Enumerable.FirstOrDefault(_viewModel.Meetings, m => m.Id == previousSelectedId.Value);
                }

                RefreshTabLanguageAndTables();
            }
        }

        private void RefreshTabLanguageAndTables()
        {
            if (TranslationSource.Instance.CurrentCulture != null)
            {
                this.Language = XmlLanguage.GetLanguage(TranslationSource.Instance.CurrentCulture.Name);
            }

            if (MeetingsDataGrid != null && MeetingsDataGrid.ItemsSource != null)
            {
                MeetingsDataGrid.Items.Refresh();
            }

            if (TiedDateComboBox != null && TiedDateComboBox.ItemsSource != null)
            {
                TiedDateComboBox.Items.Refresh();
            }
        }
    }
}