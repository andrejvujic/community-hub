using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using CommunityHub.Ui.Resources.Localization;
using CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Tabs;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Tabs
{
    public partial class ProfileTab : UserControl
    {
        private readonly ProfileTabViewModel _viewModel;

        public ProfileTab()
        {
            InitializeComponent();

            _viewModel = new ProfileTabViewModel();
            DataContext = _viewModel;

            this.IsVisibleChanged += ProfileTab_IsVisibleChanged;

            TranslationSource.Instance.PropertyChanged += (s, e) =>
            {
                if (this.IsVisible)
                {
                    RefreshProfileLanguageAndRatings();
                }
            };
        }

        public void Initialize(long coordinatorId)
        {
            _viewModel.Initialize(coordinatorId);
        }

        private void ProfileTab_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                _viewModel.LoadData();
                RefreshProfileLanguageAndRatings();
            }
        }

        private void RefreshProfileLanguageAndRatings()
        {
            if (TranslationSource.Instance.CurrentCulture != null)
            {
                this.Language = XmlLanguage.GetLanguage(TranslationSource.Instance.CurrentCulture.Name);
            }

            var currentContext = this.DataContext;
            this.DataContext = null;
            this.DataContext = currentContext;
        }
    }
}