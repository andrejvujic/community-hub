using System.Windows.Controls;
using System.Windows.Markup;
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
        }

        public void Initialize(long coordinatorId)
        {
            _viewModel.Initialize(coordinatorId);
        }
    }
}