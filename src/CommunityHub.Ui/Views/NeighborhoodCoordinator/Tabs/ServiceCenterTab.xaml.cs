using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using CommunityHub.Ui.Resources.Localization;
using CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Tabs;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Tabs
{
    public partial class ServiceCenterTab : UserControl
    {
        private readonly ServiceCenterTabViewModel _viewModel = null!;

        public ServiceCenterTab()
        {
            InitializeComponent();
            Language = XmlLanguage.GetLanguage("sr-Latn-RS");

            _viewModel = new ServiceCenterTabViewModel();
            DataContext = _viewModel;
            this.IsVisibleChanged += ServiceCenterTab_IsVisibleChanged;

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
            if (e.Key == Key.V && (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
                e.Handled = true;
        }

        private void ServiceCenterTab_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
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

            _viewModel.RefreshLocalizedStrings();
        }
    }
}