using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using CommunityHub.Ui.Resources.Localization;
using CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Tabs;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Tabs
{
    public partial class CityFacilitiesTab : UserControl
    {
        private readonly CityFacilitiesTabViewModel _viewModel;

        public CityFacilitiesTab()
        {
            InitializeComponent();

            _viewModel = new CityFacilitiesTabViewModel();
            DataContext = _viewModel;
            this.IsVisibleChanged += CityFacilitiesTab_IsVisibleChanged;

            TranslationSource.Instance.PropertyChanged += (s, e) =>
            {
                if (this.IsVisible)
                {
                    RefreshLanguageAndBindings();
                }
            };
        }

        public void Initialize(long coordinatorId)
        {
            _viewModel.Initialize(coordinatorId);
        }

        private void NumberTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(char.IsDigit);
        }

        private void NumberTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.V &&
                (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
                e.Handled = true;
        }

        private void CityFacilitiesTab_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                RefreshLanguageAndBindings();
                _viewModel.LoadNeighborhoods();
            }
        }

        private void RefreshLanguageAndBindings()
        {
            var currentCultureName = TranslationSource.Instance.CurrentCulture?.Name ?? "sr-Latn-RS";
            this.Language = XmlLanguage.GetLanguage(currentCultureName);

            var tempReservationFrom = _viewModel.ReservationStartDate;
            var tempReservationTo = _viewModel.ReservationEndDate;
            var tempReportFrom = _viewModel.ReportStartDate;
            var tempReportTo = _viewModel.ReportEndDate;

            _viewModel.ReservationStartDate = null;
            _viewModel.ReservationEndDate = null;
            _viewModel.ReportStartDate = null;
            _viewModel.ReportEndDate = null;

            _viewModel.ReservationStartDate = tempReservationFrom;
            _viewModel.ReservationEndDate = tempReservationTo;
            _viewModel.ReportStartDate = tempReportFrom;
            _viewModel.ReportEndDate = tempReportTo;
        }
    }
}