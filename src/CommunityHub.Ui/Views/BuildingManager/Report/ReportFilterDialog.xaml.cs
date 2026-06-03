using System.Windows;
using CommunityHub.Ui.ViewModels.BuildingManager.Report;

namespace CommunityHub.Ui.Views.BuildingManager.Report
{
    public partial class ReportFilterDialog : Window
    {
        public ReportFilterDialog(ReportFilterDialogViewModel _viewModel)
        {
            InitializeComponent();
            DataContext = _viewModel;
        }
    }
}