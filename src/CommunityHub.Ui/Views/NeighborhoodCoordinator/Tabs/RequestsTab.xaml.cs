using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using CommunityHub.Ui.Resources.Localization;
using CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Tabs;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Tabs
{
    public partial class RequestsTab : UserControl
    {
        private readonly RequestsTabViewModel _viewModel;

        public RequestsTab()
        {
            InitializeComponent();

            _viewModel = new RequestsTabViewModel();
            DataContext = _viewModel;
            this.IsVisibleChanged += RequestsTab_IsVisibleChanged;

            TranslationSource.Instance.PropertyChanged += (s, e) =>
            {
                if (this.IsVisible)
                {
                    RefreshRequestsLanguageAndTable();
                }
            };
        }

        public void Initialize(long coordinatorId)
        {
            _viewModel.Initialize(coordinatorId);
        }

        private void RequestsTab_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                RefreshRequestsLanguageAndTable();
            }
        }

        private void RefreshRequestsLanguageAndTable()
        {
            if (TranslationSource.Instance.CurrentCulture != null)
            {
                this.Language = XmlLanguage.GetLanguage(TranslationSource.Instance.CurrentCulture.Name);
            }

            if (RequestsDataGrid != null && RequestsDataGrid.ItemsSource != null)
            {
                RequestsDataGrid.Items.Refresh();
            }
        }
    }
}