using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using CommunityHub.Ui.Resources.Localization;
using CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Tabs;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Tabs
{
    public partial class NeighborhoodBudgetTab : UserControl
    {
        private readonly NeighborhoodBudgetViewModel _viewModel;

        public NeighborhoodBudgetTab()
        {
            InitializeComponent();
            Language = XmlLanguage.GetLanguage("sr-Latn-RS");

            _viewModel = new NeighborhoodBudgetViewModel();
            DataContext = _viewModel;

            ExpenseDatePicker.SelectedDate = DateTime.Now;
            this.IsVisibleChanged += NeighborhoodBudgetTab_IsVisibleChanged;

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

        private void NeighborhoodBudgetTab_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
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

            BudgetCategoriesDataGrid?.Items.Refresh();
            ExpensesDataGrid?.Items.Refresh();
            DonationsDataGrid?.Items.Refresh();
        }
    }
}