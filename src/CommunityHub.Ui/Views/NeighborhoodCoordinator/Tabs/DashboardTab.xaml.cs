using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using CommunityHub.Ui.Resources.Localization;
using CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Tabs;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Tabs
{
    public partial class DashboardTab : UserControl
    {
        private readonly DashboardTabViewModel _viewModel;

        public DashboardTab()
        {
            InitializeComponent();

            _viewModel = new DashboardTabViewModel();
            DataContext = _viewModel;
            this.IsVisibleChanged += DashboardTab_IsVisibleChanged;

            TranslationSource.Instance.PropertyChanged += (s, e) =>
            {
                if (this.IsVisible)
                {
                    RefreshLanguageAndDatePickers();
                }
            };
        }

        public void Initialize(long userId,
            Action<long, long> navigateToBudget,
            Action<long> navigateToMeetings,
            Action navigateToRequests)
        {
            _viewModel.Initialize(userId, navigateToBudget, navigateToMeetings, navigateToRequests);
            Window.GetWindow(this).KeyDown += DashboardTab_KeyDown;
        }

        private void FilterToggle_Changed(object sender, RoutedEventArgs e)
        {
            _viewModel.RefreshFilters();
        }

        private void DashboardTab_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                _viewModel.LoadDashboard();
                RefreshLanguageAndDatePickers();
            }
        }

        private void RefreshLanguageAndDatePickers()
        {
            if (TranslationSource.Instance.CurrentCulture != null)
            {
                this.Language = XmlLanguage.GetLanguage(TranslationSource.Instance.CurrentCulture.Name);

                var tempFrom = _viewModel.ReportFrom;
                var tempTo = _viewModel.ReportTo;

                _viewModel.ReportFrom = null;
                _viewModel.ReportTo = null;

                _viewModel.ReportFrom = tempFrom;
                _viewModel.ReportTo = tempTo;
            }
        }

        private void DashboardTab_KeyDown(object sender, KeyEventArgs e)
        {
            if (!IsVisible) return;
            if (!Keyboard.IsKeyDown(Key.LeftCtrl) && !Keyboard.IsKeyDown(Key.RightCtrl)) return;
 
            switch (e.Key)
            {
                case Key.N:
                    if (_viewModel.RegisterNeighborhoodCommand.CanExecute(null))
                        _viewModel.RegisterNeighborhoodCommand.Execute(null);
                    break;
                case Key.I:
                    if (_viewModel.GenerateReportCommand.CanExecute(null))
                        _viewModel.GenerateReportCommand.Execute(null);
                    break;
                case Key.S:
                    _viewModel.ShowServiceFilter = !_viewModel.ShowServiceFilter;
                    break;
                case Key.M:
                    _viewModel.ShowMeetingsFilter = !_viewModel.ShowMeetingsFilter;
                    break;
                case Key.D:
                    _viewModel.ShowDonationsFilter = !_viewModel.ShowDonationsFilter;
                    break;
            }
        }
    }
}