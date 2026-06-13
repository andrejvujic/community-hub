using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Tabs;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Tabs
{
    public partial class CityFacilitiesTab : UserControl
    {
        private readonly CityFacilitiesTabViewModel _viewModel;

        public CityFacilitiesTab()
        {
            InitializeComponent();
            Language = XmlLanguage.GetLanguage("sr-Latn-RS");

            _viewModel = new CityFacilitiesTabViewModel();
            DataContext = _viewModel;
            this.IsVisibleChanged += CityFacilitiesTab_IsVisibleChanged;
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
                _viewModel.LoadNeighborhoods();
            }
        }
    }
}